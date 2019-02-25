using System;

namespace H.Framework.Data.ORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DetailListAttribute : Attribute
    {
        /// <summary>
        /// 多对多中间表名
        /// </summary>
        public string TableName { set; get; }

        /// <summary>
        /// 多对多中间表对应的字段名
        /// </summary>
        public string ForeignKeyIDName { set; get; }

        /// <summary>
        /// 多对多中间表对应的字段名
        /// </summary>
        public string ForeignKeyIDName2 { set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">多对多中间表名</param>
        /// <param name="foreignKeyIDName">多对多中间表对应的字段名</param>
        /// <param name="foreignKeyIDName2">多对多中间表对应的字段名</param>
        public DetailListAttribute(string tableName = "", string foreignKeyIDName = "", string foreignKeyIDName2 = "")
        {
            TableName = tableName;
            ForeignKeyIDName = foreignKeyIDName;
            ForeignKeyIDName2 = foreignKeyIDName2;
        }
    }
}