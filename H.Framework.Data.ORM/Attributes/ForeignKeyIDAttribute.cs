using System;

namespace H.Framework.Data.ORM.Attributes
{
    /// <summary>
    /// 一对多，在外键表设置ForeignKeyID
    /// </summary>
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