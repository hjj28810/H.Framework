using System;

namespace H.Framework.Data.ORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DataFieldAttribute : Attribute
    {
        /// <summary>
        /// 表映射的字段名
        /// </summary>
        public string ColumnName { set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="columnName">表映射的字段名</param>
        public DataFieldAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}