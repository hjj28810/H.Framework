using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H.Framework.Core.Log
{
    public class LogMessage<T>
    {
        public LogMessage()
        {
            TimeStamp = DateTime.Now.ToString("O");
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
        Business
    }
}