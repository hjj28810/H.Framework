using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace H.Framework.Core.Utilities
{
    public static class RegexExtensions
    {
        /// <summary>
        /// 匹配相应字符的数量
        /// </summary>
        /// <param name="str"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static int CharNum(this string str, string search)
        {
            var count = 0;
            if (!string.IsNullOrWhiteSpace(@str) && !string.IsNullOrEmpty(@search))
                count = Regex.Matches(str, @search).Count;
            return count;
        }

        /// <summary>
        /// 校验手机号码是否符合标准。
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool ValidateMobile(this string mobile)
        {
            if (string.IsNullOrEmpty(mobile))
                return false;
            return Regex.IsMatch(mobile, @"^(13|14|15|16|18|19|17)\d{9}$");
        }

        private static string DatePattern(char separator)
        {
            //return @"(?:(?!0000)[0-9]{4}" + separator + "(?:(?:0[1-9]|1[0-2])" + separator + "(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])" + separator + "(?:29|30)|(?:0[13578]|1[02])" + separator + "31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)" + separator + "02" + separator + "29)";
            return @"(?:(?!0000)[0-9]{4}" + separator + "(?:(?:0?[1-9]|1[0-2])" + separator + "(?:0?[1-9]|1[0-9]|2[0-8])|(?:0?[13-9]|1[0-2])" + separator + "(?:29|30)|(?:0?[13578]|1[02])" + separator + "31)|(?:[0-9]{2}(?:0?[48]|[2468][048]|[13579][26])|(?:0?[48]|[2468][048]|[13579][26])00)(" + separator + "2|" + separator + "02)" + separator + "29)";
        }

        private static string TimePattern()
        {
            return @"((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)";
        }

        private static string DateTimePattern(char separator)
        {
            return @DatePattern(separator) + " " + @TimePattern();
        }

        private static string DateTimePattern()
        {
            return @"(0\d|1[0-2])\/([0-2]\d|3[01])\/[12]\d{3}\s+([01][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]\s+((a|p)m|(A|P)M)";
        }

        public static bool IsMatchDate(this string str, char separator = '-')
        {
            return Regex.IsMatch(str, DatePattern(separator));
        }

        public static bool IsMatchTime(this string str)
        {
            return Regex.IsMatch(str, TimePattern());
        }

        public static bool IsMatchDateTime(this string str, char separator = '-')
        {
            return Regex.IsMatch(str, DateTimePattern(separator));
        }

        private static List<string> MatchList(MatchCollection collection)
        {
            var strList = new List<string>();
            for (int i = 0; i < collection.Count; i++)
            {
                //var gc = collection[i].Groups; //得到所有分组
                //for (int j = 0; j < gc.Count; j++) //多分组 匹配的原始文本不要
                //{
                //    string temp = gc[j].Value;
                //    if (!string.IsNullOrEmpty(temp))
                //    {
                //        strList.Add(temp); //获取结果   strList中为匹配的值
                //    }
                //}
                if (!string.IsNullOrEmpty(collection[i].Value) && !strList.Any(x => x == collection[i].Value))
                {
                    strList.Add(collection[i].Value); //获取结果   strList中为匹配的值
                }
            }
            return strList;
        }

        public static List<string> MatchsDate(this string str, char separator = '-')
        {
            return MatchList(Regex.Matches(str, DatePattern(separator)));
        }

        public static List<string> MatchsTime(this string str)
        {
            return MatchList(Regex.Matches(str, TimePattern()));
        }

        public static List<string> MatchsDateTime(this string str, char separator = '-')
        {
            return MatchList(Regex.Matches(str, DateTimePattern(separator)));
        }

        public static List<string> MatchsDateTime(this string str)
        {
            return MatchList(Regex.Matches(str, DateTimePattern()));
        }
    }
}