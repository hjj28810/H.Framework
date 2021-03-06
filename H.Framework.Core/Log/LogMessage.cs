﻿using Newtonsoft.Json;
using System;

namespace H.Framework.Core.Log
{
    public class LogMessage<T>
    {
        public LogMessage()
        {
            TimeStamp = DateTimeOffset.Now.ToString("O");
        }

        public string Title { get; set; }

        public string TimeStamp { get; set; }

        public T Data { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public enum LogType
    {
        Error,
        Business,
        System,
        Other,
        Aliyun
    }
}