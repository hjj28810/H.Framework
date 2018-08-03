using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace H.Framework.WPF.Infrastructure.Converters
{
    public class ObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] parray = parameter.ToString().ToLower().Split(':'); //将参数字符分段 parray[0]为比较值，parray[1]为true返回值，parray[2]为false返回值
            if (value == null)
                return parray[2];  //如果数据源为空，默认返回false返回值
            if (parray[0].Contains("|"))  //判断有多个比较值的情况
                return parray[0].Split('|').Contains(value.ToString().ToLower()) ? parray[1] : parray[2];  //多值比较
            return parray[0].Equals(value.ToString().ToLower()) ? parray[1] : parray[2];  //单值比较
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var returnValue = "otherValue";
            string[] parray = parameter.ToString().ToLower().Split(':');
            if (value == null)
                return returnValue;
            var valueStr = value.ToString().ToLower();
            if (valueStr != parray[1])
                return returnValue;
            else
                return parray[0].Contains('|') ? parray[0].Split('|')[0] : parray[0];
        }
    }
}