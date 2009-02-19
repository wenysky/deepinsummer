using System;
using System.Collections.Generic;
using System.Text;

namespace Natsuhime.Proxy
{
    public class ProxyUtility
    {
        public static string GetCurrentIP_RegexPage(string pagesource, string regexString)
        {
            return RegexUtility.GetMatch(pagesource, regexString);
        }
    }
}
