using System;
using System.Net;

namespace Natsuhime.LoginUtility
{
    public class LoginDZ
    {
        /// <summary>
        /// ��¼DZ
        /// </summary>
        /// <param name="Url">logging.php��ַ.�븽��?action=login���</param>
        /// <param name="UID">UID</param>
        /// <param name="Password">����</param>
        /// <param name="VCode">��֤��(��ʱδ֧��)</param>
        /// <param name="Questionid">��¼��ʾ����id</param>
        /// <param name="Answer">��</param>
        /// <param name="Charset">��ҳ����</param>
        /// <param name="Proxy">����(��ʹ���봫��null)</param>
        /// <returns></returns>
        public static CookieContainer Login(string Url, string UID, string Password, string VCode, string Questionid, string Answer, string Charset, WebProxy Proxy)
        {
            string returnData = "";
            string formhash = "";
            CookieContainer objCookie = new CookieContainer();
            Httper objPostHttper = new Httper();
            objPostHttper.Url = Url;
            objPostHttper.Charset = Charset;
            objPostHttper.Cookie = objCookie;
            if (Proxy != null)
            {
                objPostHttper.Proxy = Proxy;
            }

            try
            {
                formhash = RegexUtility.GetMatch(objPostHttper.HttpGet(), "formhash=(.*)\"");

                objPostHttper.PostData = string.Format("&formhash={0}&referer=index.php&loginfield=uid&username={1}&password={2}&questionid={3}&answer={4}&cookietime=2592000&styleid=&loginsubmit=%CC%E1%BD%BB"
                    , formhash, UID, Password, Questionid, Answer);
                returnData = objPostHttper.HttpPost();
                if (returnData.IndexOf("��ӭ������") > 0)
                {
                    //��¼�ɹ�,����cookie
                    return objCookie;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}
