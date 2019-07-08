using System;
using System.Collections.Generic;
using System.Text;

namespace H.Framework.UMeng.Push.Bases
{
    public interface IPush
    {
        ReturnJsonClass SendMessage<T>(PostUMengJson<T> paramsJsonObj) where T : class, new();

        void AsynSendMessage<T>(PostUMengJson<T> paramsJsonObj, Action<ReturnJsonClass> callback) where T : class, new();
    }
}