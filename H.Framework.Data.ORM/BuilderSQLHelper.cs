﻿using H.Framework.Core.Utilities;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace H.Framework.Data.ORM
{
    public static class BuilderSQLHelper
    {
        private static readonly string[] _keyArr = new string[] { "from", "order", "where", "select", "between" };

        public static StringBuilder ReplaceSQLKW(this StringBuilder builder)
        {
            return builder.Equal().And().Or().DateTimeFormat().DateTimeFormatSP().RemoveQuotes();
        }

        public static string ReplaceSQLKW(this string str)
        {
            return str.In().Like().LikeStart().LikeEnd();
        }

        public static string ReplaceKeyword(this string str)
        {
            foreach (var item in _keyArr)
            {
                str = str.Replace("," + item + ",", ",`" + item.TrimStart(',').TrimEnd(',') + "`,");
                if (str.StartsWith(item + ","))
                {
                    str = "`" + item + "`" + str.Substring(item.Length, str.Length - item.Length);
                }
                if (str.EndsWith("," + item))
                {
                    str = str.Substring(0, str.Length - item.Length) + "`" + item + "`";
                }
            }

            return str;
        }

        public static StringBuilder RemoveQuotes(this StringBuilder builder)
        {
            return builder.Replace("\"", "'");
        }

        public static StringBuilder DateTimeFormat(this StringBuilder builder)
        {
            var datelist = builder.ToString().MatchsDateTime('/');
            foreach (var item in datelist)
                builder = builder.Replace(item, "'" + item + "'");
            //var datelist1 = builder.ToString().MatchsDate('/');
            //foreach (var item in datelist1)
            //    builder = builder.Replace(item, "'" + item + "'");
            //var datelist2 = builder.ToString().MatchsDate('-');
            //foreach (var item in datelist2)
            //    builder = builder.Replace(item, "'" + item + "'");
            var datelist3 = builder.ToString().MatchsDateTime('-');
            foreach (var item in datelist3)
                builder = builder.Replace(item, "'" + item + "'");
            return builder;
        }

        public static StringBuilder DateTimeFormatSP(this StringBuilder builder)
        {
            var datelist = builder.ToString().MatchsDateTime();
            foreach (var item in datelist)
                builder = builder.Replace(item, "'" + DateTime.Parse(item).ToString("yyyy-MM-dd HH:mm:ss") + "'");
            return builder;
        }

        public static StringBuilder Equal(this StringBuilder builder)
        {
            return builder.Replace("==", "=");
        }

        public static StringBuilder Or(this StringBuilder builder)
        {
            return builder.Replace("OrElse", "or");
        }

        public static StringBuilder And(this StringBuilder builder)
        {
            return builder.Replace("AndAlso", "and");
        }

        public static string In(this string str)
        {
            var pattern = @".Contains[\(]'(.*?)'[\)]";
            var matchs = Regex.Matches(str, pattern);
            foreach (Match m in matchs)
            {
                var newStr = m.Value.Replace(".Contains('", " in (").Replace("')", ")").Replace("''", "'");
                str = str.Replace(m.Value, newStr);
            }
            return str;
        }

        public static string LikeStart(this string str)
        {
            var pattern = @".StartsWith[\(]'(.*?)'[\)]";
            var matchs = Regex.Matches(str, pattern);
            foreach (Match m in matchs)
            {
                var newStr = m.Value.Replace(".StartsWith('", " like ('").Replace("')", "%')");
                str = str.Replace(m.Value, newStr);
            }
            return str;
        }

        public static string LikeEnd(this string str)
        {
            return str.Replace(".EndsWith('", " like ('%");
        }

        public static string Like(this string str)
        {
            var pattern = @".Equals[\(]'(.*?)'[\)]";
            var matchs = Regex.Matches(str, pattern);
            foreach (Match m in matchs)
            {
                var newStr = m.Value.Replace(".Equals('", " like ('%").Replace("')", "%')");
                str = str.Replace(m.Value, newStr);
            }
            return str;
        }

        //public static bool Contains(this string str, params string[] condition)
        //{
        //    return true;
        //}
    }
}