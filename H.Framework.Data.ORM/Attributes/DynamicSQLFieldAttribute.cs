using System;
using System.Collections.Generic;
using System.Text;

namespace H.Framework.Data.ORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DynamicSQLFieldAttribute : Attribute
    {
        /// <summary>
        /// 动态sql语句
        /// </summary>
        public string SQLString { set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlString">动态sql语句</param>
        public DynamicSQLFieldAttribute(string sqlString)
        {
            SQLString = sqlString;
        }
    }
}