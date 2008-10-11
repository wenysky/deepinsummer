using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

namespace Natsuhime
{
    public class NewTemplate
    {
        static Regex[] r = new Regex[23];
        static NewTemplate()
        {
            RegexOptions options = RegexOptions.Compiled;

            //解析模板引用
            r[0] = new Regex(@"<%template ([^\[\]\{\}\s]+)%>", options);

            //解析循环loop
            r[1] = new Regex(@"<%loop ((\(([a-zA-Z]+)\) )?)([^\[\]\{\}\s]+) ([^\[\]\{\}\s]+)%>", options);
            r[2] = new Regex(@"<%\/loop%>", options);

            //解析while条件
            r[3] = new Regex(@"<%while ([^\[\]\{\}\s]+)%>", options);
            r[4] = new Regex(@"<%\/while ([^\[\]\{\}\s]+)%>", options);

            //解析if..else条件
            r[5] = new Regex(@"<%if (?:\s*)(([^\s]+)((?:\s*)(\|\||\&\&)(?:\s*)([^\s]+))?)(?:\s*)%>", options);
            //r[5] = new Regex(@"<%if ([^\s]+)%>", options);
            r[6] = new Regex(@"<%else(( (?:\s*)if (?:\s*)(([^\s]+)((?:\s*)(\|\||\&\&)(?:\s*)([^\s]+))?))?)(?:\s*)%>", options);
            r[7] = new Regex(@"<%\/if%>", options);

            //解析{var.a}
            r[8] = new Regex(@"(\{strtoint\(([^\s]+?)\)\})", options);

            //解析{request[a]}
            r[9] = new Regex(@"(<%urlencode\(([^\s]+?)\)%>)", options);

            //解析{var[a]}
            r[10] = new Regex(@"(<%datetostr\(([^\s]+?),(.*?)\)%>)", options);
            r[11] = new Regex(@"(\{([^\.\[\]\{\}\s]+)\.([^\[\]\{\}\s]+)\})", options);

            //解析普通变量{}
            r[12] = new Regex(@"(\{request\[([^\[\]\{\}\s]+)\]\})", options);

            //解析==表达式
            r[13] = new Regex(@"(\{([^\[\]\{\}\s]+)\[([^\[\]\{\}\s]+)\]\})", options);

            //解析==表达式
            r[14] = new Regex(@"({([^\[\]/\{\}='\s]+)})", options);

            //解析普通变量{}
            r[15] = new Regex(@"({([^\[\]/\{\}='\s]+)})", options);

            //解析==表达式
            r[16] = new Regex(@"(([=|>|<|!]=)\\" + "\"" + @"([^\s]*)\\" + "\")", options);

            //命名空间
            r[17] = new Regex(@"<%namespace (?:""?)([\s\S]+?)(?:""?)%>", options);

            //C#代码
            r[18] = new Regex(@"<%csharp%>([\s\S]+?)<%/csharp%>", options);

            //set标签
            r[19] = new Regex(@"<%set ((\(([a-zA-Z]+)\))?)(?:\s*)\{([^\s]+)\}(?:\s*)=(?:\s*)(.*?)(?:\s*)%>", options);

            //截取字符串
            r[20] = new Regex(@"(<%getsubstring\(([^\s]+?),(.\d*?),(.\d*?),([^\s]+?)\)%>)", options);

            //repeat标签
            r[21] = new Regex(@"<%repeat\(([^\s]+?)(?:\s*),(?:\s*)([^\s]+?)\)%>", options);

            //继承类Inherits
            r[22] = new Regex(@"<%inherits (?:""?)([\s\S]+?)(?:""?)%>", options);
        }

        public void CreateFromFolder(string templatefilefolder, string pagefilefolder)
        {
            throw new System.NotImplementedException();
        }
        //public void CreateFromFile(string templatefilepath, string pagefilepath)
        //{
        //    throw new System.NotImplementedException();
        //}
        //public void CreateFromFile(List<string> templatefilepath, string pagefilepath)
        //{
        //    throw new System.NotImplementedException();
        //}

        void Create(string templatefilepath, string pagefilepath)
        {
            Create(templatefilepath, pagefilepath, 1);
        }
        void Create(string templatefilepath, string pagefilepath, int layer)
        {
        }

        string ConvertTags(string source, int layer)
        {
            return "";
        }
    }
}
