using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H.Framework.Core.Utilities
{
    public class EqualityComparerUtility<T> : IEqualityComparer<T>
    {
        private string _propName;

        public EqualityComparerUtility(string propName)
        {
            _propName = propName;
            if (string.IsNullOrWhiteSpace(_propName))
                throw new ArgumentException("_propName不能为空");
        }

        public bool Equals(T x, T y)
        {
            var property = x.GetType().GetProperty(_propName);
            if (property == null)
                throw new ArgumentException("找不到这个" + _propName + "属性");
            return property?.GetValue(x) == property?.GetValue(y);
        }

        public int GetHashCode(T obj)
        {
            if (obj == null)
                return 0;
            return obj.GetType().GetProperty(_propName).GetHashCode();
        }
    }
}