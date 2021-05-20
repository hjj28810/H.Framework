using System;
using System.Collections.Generic;
using System.Text;

namespace H.Framework.Data.ORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OnlyQueryAttribute : Attribute
    {
    }
}