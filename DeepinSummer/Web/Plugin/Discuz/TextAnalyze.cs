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
                    threads.Add(m.Groups[2].Value, m.Groups[1].Value);
                }
            }

            return threads;
        }
    }
}
