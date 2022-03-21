using H.Framework.Core.Utilities;
using H.Framework.UMeng.Push.Bases;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace H.Framework.UMeng.Push
{
    /// <summary>
    /// 友盟消息推送
    /// create by jasnature
    /// </summary>
    public class UMengMessagePush : IPush, IDisposable
    {
        #region 内部字段

        private RestClient _requestClient;

        private static volatile Dictionary<Type, PropertyInfo[]> _cacheParamType = null;

        private readonly ReaderWriterLock _lockrw = null;

        private const string _requestProtocol = "http";

        private const string _requestMethod = "POST";

        private const string _hostUrl = "msg.umeng.com";

        private const string _postPath = "api/send";

        private string _apiFullUrl = null;

        private readonly string _appkey = null;

        private readonly string _appMasterSecret = null;

        protected const string USER_AGENT = "Push-Server/4.5";

        #endregion 内部字段

        #region 公共方法

        /// <summary>
        /// 使用默认的参数构造,参数从友盟网站的应用信息中获取
        /// </summary>
        /// <param name="appkey">appkey</param>
        /// <param name="appMasterSecret">App Master Secret，供API对接友盟服务器使用的密钥</param>
        public UMengMessagePush(string appkey, string appMasterSecret)
        {
            this._apiFullUrl = string.Concat(_requestProtocol, "://", _hostUrl, "/", _postPath);
            this._appkey = appkey;
            this._appMasterSecret = appMasterSecret;
            _cacheParamType = new Dictionary<Type, PropertyInfo[]>(10);
            this._lockrw = new ReaderWriterLock();
        }

        /// <summary>
        /// 推送消息，注意如果初始化本类已经填写了appkey，
        /// <paramref name="paramsJsonObj"/> 里面的appkey会自动赋值
        /// 反之如果您填写了<paramref name="paramsJsonObj"/> 里面的appkey，将采用参数里面的值，忽略本类初始化值。
        /// </summary>
        /// <param name="paramsJsonObj"></param>
        /// <returns></returns>
        public ReturnJsonClass SendMessage<T>(PostUMengJson<T> paramsJsonObj) where T : class, new()
        {
            var req = CreateHttpRequest(paramsJsonObj);
            var resultResponse = _requestClient.ExecuteAsync(req).Result;
            return resultResponse.Content.ToJsonObj<ReturnJsonClass>();
        }

        /// <summary>
        /// 异步推送消息，注意如果初始化本类已经填写了appkey，
        /// <paramref name="paramsJsonObj"/> 里面的appkey会自动赋值
        /// 反之如果您填写了<paramref name="paramsJsonObj"/> 里面的appkey，将采用参数里面的值，忽略本类初始化值。
        /// </summary>
        /// <param name="paramsJsonObj"></param>
        /// <param name="callback"></param>
        public void AsynSendMessage<T>(PostUMengJson<T> paramsJsonObj, Action<ReturnJsonClass> callback) where T : class, new()
        {
            var request = CreateHttpRequest(paramsJsonObj);
            var resultResponse = _requestClient.ExecuteAsync(request).Result;
            callback?.Invoke(resultResponse.Content.ToJsonObj<ReturnJsonClass>());
        }

        #endregion 公共方法

        #region 私有辅助方法

        private RestRequest CreateHttpRequest<T>(PostUMengJson<T> paramsJsonObj) where T : class, new()
        {
            string bodyJson = InitParamsAndUrl(paramsJsonObj);

            if (_requestClient == null)
            {
                _requestClient = new RestClient(new RestClientOptions { Encoding = Encoding.UTF8, UserAgent = USER_AGENT, BaseUrl = new Uri(_apiFullUrl) });
            }
            var request = new RestRequest()
            {
                RequestFormat = DataFormat.Json,
                Method = Method.Post,
            };
            request.AddParameter("application/json", bodyJson, ParameterType.RequestBody);
            //request.AddJsonBody(paramsJsonObj);
            return request;
        }

        private string InitParamsAndUrl<T>(PostUMengJson<T> paramsJsonObj) where T : class, new()
        {
            if (string.IsNullOrEmpty(paramsJsonObj.Appkey)) paramsJsonObj.Appkey = _appkey;
            var json = paramsJsonObj.ToJson(true, CaseType.Lower);
            var calcSign = HashEncryptHepler.MD5Hash(_requestMethod + _apiFullUrl + json + _appMasterSecret).ToLower();
            _apiFullUrl = string.Format("{0}?sign={1}", _apiFullUrl, calcSign);
            return json;
        }

        /// <summary>
        /// 多线程安全缓存参数类型集合
        /// </summary>
        private PropertyInfo[] GetCacheParamType<T>(T pb)
        {
            Type pbtype = pb.GetType();
            try
            {
                if (_cacheParamType.ContainsKey(pbtype))
                {
                    return _cacheParamType[pbtype];
                }
                else
                {
                    _lockrw.AcquireWriterLock(1000);
                    if (!_cacheParamType.ContainsKey(pbtype))
                    {
                        PropertyInfo[] pis = pbtype.GetProperties().OrderBy(p => p.Name).ToArray();
                        _cacheParamType.Add(pbtype, pis);
                        return pis;
                    }
                    else
                    {
                        return _cacheParamType[pbtype];
                    }
                }
            }
            finally
            {
                if (_lockrw.IsReaderLockHeld)
                {
                    _lockrw.ReleaseReaderLock();
                }
                if (_lockrw.IsWriterLockHeld)
                {
                    _lockrw.ReleaseWriterLock();
                }
            }
        }

        #endregion 私有辅助方法

        public void Dispose()
        {
            if (_cacheParamType != null) _cacheParamType.Clear();
        }
    }

    public enum PushType
    {
        /// <summary>
        /// 单播
        /// </summary>
        UniCast,

        /// <summary>
        /// 列播(要求不超过500个device_token)
        /// </summary>
        ListCast,

        /// <summary>
        /// 文件播(多个device_token可通过文件形式批量发送）
        /// </summary>
        FileCast,

        /// <summary>
        /// 广播
        /// </summary>
        BroadCast,

        /// <summary>
        /// 组播(按照filter条件筛选特定用户群, 具体请参照filter参数)
        /// </summary>
        GroupCast,

        /// <summary>
        /// 自定义播(通过开发者自有的alias进行推送)
        /// 包括以下两种case:
        /// alias: 对单个或者多个alias进行推送
        /// file_id: 将alias存放到文件后，根据file_id来推送
        /// </summary>
        CustomizedCast
    }
}