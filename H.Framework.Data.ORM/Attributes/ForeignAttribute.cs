using System;

namespace H.Framework.Data.ORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ForeignAttribute : Attribute
    {
        /// <summary>
        /// 对应的关联表名
        /// </summary>
        public string TableName { set; get; }

        /// <summary>
        /// 对应的外键属性名
        /// </summary>
        public string ForeignKeyIDPropName { set; get; }

        /// <summary>
        /// 关联表主键名
        /// </summary>
        public string ForeignPrimaryKeyIDName { set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">对应的关联表名</param>
        /// <param name="foreignKeyIDPropName">对应的外键属性名</param>
        /// <param name="foreignPrimaryKeyIDName">关联表主键名</param>
        public ForeignAttribute(string tableName, string foreignKeyIDPropName, string foreignPrimaryKeyIDName = "")
        {
            TableName = tableName;
            ForeignKeyIDPropName = foreignKeyIDPropName;
            ForeignPrimaryKeyIDName = foreignPrimaryKeyIDName;
        }
    }
}