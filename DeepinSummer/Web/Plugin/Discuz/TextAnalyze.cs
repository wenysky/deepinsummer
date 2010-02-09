using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Natsuhime.Web.Plugin.Discuz
{
    public class TextAnalyze
    {
        public static Dictionary<string, string> GetThreadsInBoard(string sourceHtml)
        {
            Dictionary<string, string> threads = new Dictionary<string, string>();

            string regexstring = RegexStringLib.GetThreadsInBoard();
            MatchCollection mc = RegexUtility.GetMatchFull(sourceHtml, regexstring);

            if (mc != null)
            {
                foreach (Match m in mc)
                {
                    if (!threads.ContainsKey(m.Groups[1].Value))
                    {
                        threads.Add(m.Groups[1].Value, m.Groups[2].Value);
                    }
                }
            }
            return threads;
        }

        public static int GetBoardPageCount(string sourceHtml)
        {
            string regexstring = RegexStringLib.GetBoardPageCount();
            string result = RegexUtility.GetMatch(sourceHtml, regexstring);

            int pageCount;
            if (int.TryParse(result, out pageCount))
            {
                return pageCount;
            }
            else
            {
                return -1;
            }
        }
    }
}
