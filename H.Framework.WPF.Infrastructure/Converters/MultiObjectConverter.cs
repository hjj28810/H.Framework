using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace H.Framework.WPF.Infrastructure.Converters
{
    public class MultiObjectConverter : IMultiValueConverter
    {
        /// <summary>
        /// 多值转换器
        /// </summary>
        /// <param name="values">参数值数组</param>
        /// <param name="parameter">
        /// <para>参数</para>
        /// <para>各组比较值:比较条件(&amp;或|):true返回值:false返回值:返回值类型枚举</para>
        /// <para>v1;v2-1|v2-2;v3:&amp;:Visible:Collapsed:1</para>
        /// </param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string[] param = parameter.ToString().ToLower().Split(':');   //将参数字符串分段
            string[] compareValues = param[0].Split(';'); //将比较值段分割为数组
            if (values.Length != compareValues.Length)  //比较源数据和比较参数个数是否一致
                return ConvertValue(param[3], param[4]);
            var trueCount = 0; //满足条件的结果数量
            string currentValue;
            IList<string> currentParamArray;
            for (var i = 0; i < values.Length; i++)
            {
                currentValue = values[i] != null ? values[i].ToString().ToLower() : string.Empty;
                if (compareValues[i].Contains("|"))
                {
                    //当前比较值段包含多个比较值
                    currentParamArray = compareValues[i].Split('|');
                    trueCount += currentParamArray.Contains(currentValue) ? 1 : 0;  //满足条件，结果+1
                }
                else
                {
                    trueCount += compareValues[i].Equals(currentValue) ? 1 : 0;  //满足条件，结果+1
                }
            }
            var compareResult = param[1].Equals("&") ?
                trueCount == values.Length :
                trueCount > 0;   //判断比较结果
            return ConvertValue(compareResult ? param[2] : param[3], param[4]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private object ConvertValue(string result, string enumStr)
        {
            var convertResult = (ConvertResult)int.Parse(enumStr);
            if (convertResult == ConvertResult.Visibility)
                return result.Equals("collapsed") ? Visibility.Collapsed : Visibility.Visible;
            if (convertResult == ConvertResult.Boolean)
                return System.Convert.ToBoolean(result);
            return null;  //后续自行扩展
        }

        private enum ConvertResult
        {
            Visibility = 1,
            Boolean = 2,
            String = 3,
            Int = 4,
            Double = 5,
            Brush = 6,
            Style = 7,
            Template = 8
        }
    }
}