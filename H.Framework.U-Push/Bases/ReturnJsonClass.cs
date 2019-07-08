using System;
using System.Collections.Generic;
using System.Text;

namespace H.Framework.UMeng.Push.Bases
{
    public class ReturnJsonClass
    {
        /// <summary>
        /// 返回结果，"SUCCESS"或者"FAIL"
        /// </summary>
        public string Ret { get; set; }

        /// <summary>
        /// 结果具体信息
        /// </summary>
        public ResultInfo Data { get; set; }
    }

    public class ResultInfo
    {
        /// <summary>
        /// 当"ret"为"SUCCESS"时,包含如下参数:
        /// 当type为unicast、listcast或者customizedcast且alias不为空时:
        /// </summary>
        public string Msg_Id { get; set; }

        /// <summary>
        /// 当type为于broadcast、groupcast、filecast、customizedcast且file_id不为空的情况(任务)
        /// </summary>
        public string Task_id { get; set; }

        /// <summary>
        /// 当"ret"为"FAIL"时,包含如下参数:错误码详见附录I。
        /// </summary>
        public int Error_Code { get; set; }

        /// <summary>
        /// 当"ret"为"FAIL"时,包含如下参数:错误信息
        /// </summary>
        public string Error_Msg { get; set; }

        /// <summary>
        /// 如果开发者填写了thirdparty_id, 接口也会返回该值。
        /// </summary>
        public string Thirdparty_Id { get; set; }
    }
}