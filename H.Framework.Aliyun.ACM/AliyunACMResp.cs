using System;
using System.Collections.Generic;
using System.Text;

namespace H.Framework.Aliyun.ACM
{
    public class AliyunConfigurationResp
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string RequestId { get; set; }
        public AliyunConfiguration Configuration { get; set; }
    }

    //public class AliyunConfigurationListener
    //{
    //    public string ConfigInfo { get; set; }
    //}

    public class AliyunConfiguration
    {
        public string AppName { get; set; }
        public string Content { get; set; }
        public string DataId { get; set; }
        public string Desc { get; set; }
        public string Group { get; set; }
        public string Md5 { get; set; }
        public string Tags { get; set; }
        public string Type { get; set; }
    }
}