using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Auth;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using H.Framework.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace H.Framework.Aliyun.ACM
{
    public class AliyunRequest
    {
        private readonly string _accessKey, _secretKey, _roleName;

        public AliyunRequest(string accessKey, string secretKey)
        {
            _accessKey = accessKey;
            _secretKey = secretKey;
        }

        public AliyunRequest(string roleName)
        {
            _roleName = roleName;
        }

        protected T RequestAliyun<T>(string url, string version, MethodType methodType, string region, string domain, Dictionary<string, string> args, Dictionary<string, string> headers = null)
        {
            DefaultAcsClient client;
            if (headers == null)
                headers = new Dictionary<string, string> { { "Content-Type", "application/x-www-form-urlencoded" } };
            if (string.IsNullOrWhiteSpace(_roleName))
            {
                if (string.IsNullOrWhiteSpace(_accessKey) || string.IsNullOrWhiteSpace(_secretKey))
                    throw new ArgumentException("ACK方式参数错误");
                client = new DefaultAcsClient(DefaultProfile.GetProfile(region, _accessKey, _secretKey));
            }
            else
                client = new DefaultAcsClient(DefaultProfile.GetProfile(region), new InstanceProfileCredentialsProvider(_roleName));
            var request = new CommonRequest
            {
                Method = methodType,
                Domain = domain,
                Version = version,
                UriPattern = url
            };
            // request.Protocol = ProtocolType.HTTP;
            foreach (var item in args)
                request.AddQueryParameters(item.Key, item.Value);
            foreach (var item in headers)
                request.AddHeadParameters(item.Key, item.Value);
            request.SetContent(System.Text.Encoding.Default.GetBytes(""), "utf-8", FormatType.JSON);
            var response = client.GetCommonResponse(request);
            return response.Data.ToJsonObj<T>();
        }
    }
}