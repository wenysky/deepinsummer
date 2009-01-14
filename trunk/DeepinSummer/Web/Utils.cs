using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Natsuhime.Web
{
    public class Utils
    {        
        /// <summary>
        /// 获得伪静态页码显示链接
        /// </summary>
        /// <param name="currentpage">当前页数</param>
        /// <param name="pagecount">总页数</param>
        /// <param name="pageurl">超级链接页面地址(文件名)</param>
        /// <param name="pageextname">超级链接页面地址(扩展名,带.号)</param>
        /// <param name="extendpage">周边页码显示个数上限</param>
        /// <returns>html代码</returns>
        public static string GetStaticPageNumbersHtml(int currentpage, int pagecount, string pageurl, string pageextname, int extendpage)
        {
            int startPage = 1;
            int endPage = 1;

            string t1 = "<a href=\"" + pageurl + "-1" + pageextname + "\">首页&laquo;</a>";
            string t2 = "<a href=\"" + pageurl + "-" + pagecount + pageextname + "\">&raquo;末页</a>";

            if (pagecount < 1) pagecount = 1;
            if (extendpage < 3) extendpage = 2;

            if (pagecount > extendpage)
            {
                if (currentpage - (extendpage / 2) > 0)
                {
                    if (currentpage + (extendpage / 2) < pagecount)
                    {
                        startPage = currentpage - (extendpage / 2);
                        endPage = startPage + extendpage - 1;
                    }
                    else
                    {
                        endPage = pagecount;
                        startPage = endPage - extendpage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendpage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = pagecount;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == currentpage)
                {
                    s.Append("<span>");
                    s.Append(i);
                    s.Append("</span>");
                }
                else
                {
                    s.Append("<a href=\"");
                    s.Append(pageurl);
                    s.Append("-");
                    s.Append(i);
                    s.Append(pageextname);
                    s.Append("\">");
                    s.Append(i);
                    s.Append("</a>");
                }
            }
            s.Append(t2);

            return s.ToString();
        }
        /// <summary>
        /// 获得页码显示链接
        /// </summary>
        /// <param name="currentpage">当前页数</param>
        /// <param name="pagecount">总页数</param>
        /// <param name="pageurl">超级链接地址(带文件名和公用不变的参数)</param>
        /// <param name="extendpage">周边页码显示个数上限</param>
        /// <param name="pagepramname">页码参数标记</param>
        /// <param name="anchor">锚点</param>
        /// <returns>Html代码</returns>
        public static string GetPageNumbersHtml(int currentpage, int pagecount, string pageurl, int extendpage, string pagepramname, string anchor)
        {
            if (pagepramname == "")
                pagepramname = "page";
            int startPage = 1;
            int endPage = 1;

            if (pageurl.IndexOf("?") > 0)
            {
                pageurl = pageurl + "&";
            }
            else
            {
                pageurl = pageurl + "?";
            }

            string t1 = "<a href=\"" + pageurl + "&" + pagepramname + "=1";
            string t2 = "<a href=\"" + pageurl + "&" + pagepramname + "=" + pagecount;
            if (anchor != null)
            {
                t1 += anchor;
                t2 += anchor;
            }
            t1 += "\">首页&laquo;</a>";
            t2 += "\">&raquo;末页</a>";

            if (pagecount < 1)
                pagecount = 1;
            if (extendpage < 3)
                extendpage = 2;

            if (pagecount > extendpage)
            {
                if (currentpage - (extendpage / 2) > 0)
                {
                    if (currentpage + (extendpage / 2) < pagecount)
                    {
                        startPage = currentpage - (extendpage / 2);
                        endPage = startPage + extendpage - 1;
                    }
                    else
                    {
                        endPage = pagecount;
                        startPage = endPage - extendpage + 1;
                        t2 = "";
                    }
                }
                else
                {
                    endPage = extendpage;
                    t1 = "";
                }
            }
            else
            {
                startPage = 1;
                endPage = pagecount;
                t1 = "";
                t2 = "";
            }

            StringBuilder s = new StringBuilder("");

            s.Append(t1);
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == currentpage)
                {
                    s.Append("<span>");
                    s.Append(i);
                    s.Append("</span>");
                }
                else
                {
                    s.Append("<a href=\"");
                    s.Append(pageurl);
                    s.Append(pagepramname);
                    s.Append("=");
                    s.Append(i);
                    if (anchor != null)
                    {
                        s.Append(anchor);
                    }
                    s.Append("\">");
                    s.Append(i);
                    s.Append("</a>");
                }
            }
            s.Append(t2);

            return s.ToString();
        }


        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveUnsafeHtml(string content)
        {
            content = Regex.Replace(content, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }

        /// <summary>
        /// 返回 HTML 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string HtmlEncode(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// 返回 HTML 字符串的解码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string HtmlDecode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }
    }
}
