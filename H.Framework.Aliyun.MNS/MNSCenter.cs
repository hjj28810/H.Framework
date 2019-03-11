using System;
using System.Collections.Generic;
using System.Linq;

namespace H.Framework.Aliyun.MNS
{
    public sealed class MNSCenter
    {
        private static readonly Lazy<MNSCenter> _lazy = new Lazy<MNSCenter>(() => new MNSCenter());
        public static MNSCenter Instance => _lazy.Value;
        private static List<MNSCore> _coreDic;
        private readonly object _locker = new object();

        private MNSCenter()
        {
            _coreDic = new List<MNSCore>();
        }

        public MNSCore CoreInstance(string queueName, string accessKeyId = "", string secretAccessKey = "", string endpoint = "")
        {
            MNSCore item = null;
            lock (_locker)
            {
                item = _coreDic.FirstOrDefault(x => x.QueueName == queueName);
                if (item == null)
                {
                    item = new MNSCore(queueName, accessKeyId, secretAccessKey, endpoint);
                    _coreDic.Add(item);
                }
                return item;
            }
        }
    }
}