using System;
using System.Collections.Generic;
using System.Text;

namespace H.Framework.Data.ORM.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataTableAttribute : Attribute
    {
        /// <summary>
        /// 表映射的表名
        /// </summary>
        public string TableName { set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">表映射的表名</param>
        public DataTableAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}