using Aliyun.Acs.Core.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace H.Framework.Aliyun.ACM
{
    public class AliyunACMRequest : AliyunRequest
    {
        public AliyunACMRequest(string accessKey, string secretKey) : base(accessKey, secretKey)
        {
        }

        public AliyunACMRequest(string roleName) : base(roleName)
        {
        }

        public AliyunConfigurationResp GetACMConfig(string region, string dataID, string group, string namespaceID)
        {
            return RequestACM<AliyunConfigurationResp>("/diamond-ops/pop/configuration", MethodType.GET, region, new Dictionary<string, string>
            {
                { "DataId", dataID },
                { "Group", group },
                { "NamespaceId", namespaceID}
            });
        }

        //public string AddListener(string region, string dataID, string group, string namespaceID)
        //{
        //    return RequestACM<string>("/diamond-server/config.co", MethodType.POST, region, new Dictionary<string, string>
        //    {
        //        { "Probe-Modify-Request", dataID+"^2"+group+"^2"+namespaceID+"^1" }
        //    });
        //}

        private T RequestACM<T>(string url, MethodType methodType, string region, Dictionary<string, string> args)
        {
            return RequestAliyun<T>(url, "2020-02-06", methodType, region, "acm." + region + ".aliyuncs.com", args);
        }
    }
}