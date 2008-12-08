using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Natsuhime.Common
{
    public class Utils
    {
        /// <summary>
        /// 判断给定的字符串中的数据是不是都为数值型(字符串中逗号分隔)
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static bool IsNumericString(string strNumber)
        {
            string[] numbers = strNumber.Split(',');
            return IsNumericArray(numbers);
        }
        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string id in strNumber)
            {
                if (!IsNumeric(id))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            if (expression != null)
            {
                return IsNumeric(expression.ToString());
            }
            return false;

        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && System.Text.RegularExpressions.Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        /// <summary>
        /// 取得当前的时间戳.
        /// </summary>
        /// <returns></returns>
        public static string UnixTimestamp()
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            return timeStamp.Substring(0, timeStamp.Length - 7);
        }
        public static string Timestamp2Date(string s)
        {
            string timeStamp = s;
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult.ToString("yyyy-MM-dd");
        }

        public static string GetTimestamp(int year, int month, int day)
        {
            DateTime timeStamp0 = new DateTime(1970, 1, 1);
            DateTime timeStamp = new DateTime(year, month, day);
            long a = (timeStamp.Ticks - timeStamp0.Ticks) / 10000000 - 8 * 60 * 60;
            return a.ToString();
        }
    }
}
