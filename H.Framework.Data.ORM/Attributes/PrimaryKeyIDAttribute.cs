using System;

namespace H.Framework.Data.ORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyIDAttribute : Attribute
    {
        /// <summary>
        /// 主键对应的字段名
        /// </summary>
        public string KeyName { set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="keyName">主键对应的字段名</param>
        public PrimaryKeyIDAttribute(string keyName = "")
        {
            keyName = KeyName;
        }
    }
}