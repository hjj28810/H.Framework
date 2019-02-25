using System;

namespace H.Framework.Data.ORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ForeignKeyIDAttribute : Attribute
    {
        /// <summary>
        /// 表对应的字段名
        /// </summary>
        public string TableName { set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表对应的字段名</param>
        public ForeignKeyIDAttribute(string tableName = "")
        {
            TableName = tableName;
        }
    }
}