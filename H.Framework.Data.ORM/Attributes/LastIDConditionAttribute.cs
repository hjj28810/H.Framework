using System;

namespace H.Framework.Data.ORM.Attributes
{
    /// <summary>
    /// 查询最新自增ID的条件
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class LastIDConditionAttribute : Attribute
    {
    }
}