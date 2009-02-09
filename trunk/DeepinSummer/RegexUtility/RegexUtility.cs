using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Natsuhime
{
    public class RegexUtility
    {
        public static string GetMatch(string strSource, string strRegex)
        {
            try
            {

                Regex r;
                MatchCollection m;
                r = new Regex(strRegex, RegexOptions.IgnoreCase);
                m = r.Matches(strSource);

                if (m.Count <= 0) 
                    return "";
                else
                    return m[0].Groups[1].Value;
            }
            catch
            {
                return "";
            }
        }

        public static MatchCollection GetMatchFull(string strSource, string strRegex)
        {
            try
            {

                Regex r;
                MatchCollection m;
                r = new Regex(strRegex, RegexOptions.IgnoreCase);
                m = r.Matches(strSource);

                if (m.Count <= 0) return null;

                return m;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 正则替换
        /// </summary>
        /// <param name="pattern">替换规则</param>
        /// <param name="input">原始字符串</param>
        /// <param name="replacement">替换为</param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceRegex(string pattern, string input, string replacement)
        {
            // Regex search and replace
            RegexOptions options = RegexOptions.IgnoreCase;
            Regex regex = new Regex(pattern, options);
            return regex.Replace(input, replacement);
        }    
    }
}
