using System;

namespace H.Framework.Data.ORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DataFieldIgnoreAttribute : Attribute
    {
        /// <summary>
        /// 表对应的字段名
        /// </summary>
        public string ColumnName { set; get; }

        public DataFieldIgnoreAttribute(string columnName)
        {
            ColumnName = columnName;
        }

        public DataFieldIgnoreAttribute()
        {
        }
    }
}