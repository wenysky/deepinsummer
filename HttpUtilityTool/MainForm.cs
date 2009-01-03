using System;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.ComponentModel;

using Natsuhime;
using System.Web;
using mshtml;

namespace HttpUtilityTool
{
    public partial class MainForm : Form
    {
        NewHttper httper;
        CookieContainer cookie;
        public MainForm()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cbxPageCharset.SelectedIndex = 0;
            tcMessage.SelectedIndex = 1;
            this.cookie = new CookieContainer();
            this.httper = new NewHttper();
            this.httper.Referer = cbxUrl.Text.Trim();
            //this.httper.UserAgent = "User-Agent: Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; WOW64; SLCC1; .NET CLR 2.0.50727; .NET CLR 1.1.4322; .NET CLR 3.5.30729; .NET CLR 3.0.30618)";
            this.httper.RequestStringCompleted += new NewHttper.RequestStringCompleteEventHandler(httper_RequestStringCompleted);
            this.wbMain.Navigate("about:blank");
            this.wbMain.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wbMain_DocumentCompleted);
            this.wbMain.Navigated += new WebBrowserNavigatedEventHandler(wbMain_Navigated);
            this.wbMain.ProgressChanged += new WebBrowserProgressChangedEventHandler(wbMain_ProgressChanged);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //ShowMessage("DEBUG COOKIES");
            //ShowCookie();
            //ShowMessage("");
            if (ckbxKeepCookie.Checked)
            {
                httper.Cookie = this.cookie;
                wbMain.Document.Cookie = cookie.GetCookieHeader(new Uri(cbxUrl.Text.Trim()));
            }
            else
            {
                httper.Cookie = new CookieContainer();
                wbMain.Document.Cookie = "";
            }

            if (ckbxUserWebBrowser.Checked)
            {
                wbMain.Navigate(cbxUrl.Text.Trim());
            }
            else
            {
                httper.Url = cbxUrl.Text.Trim();
                httper.Referer = httper.Url;
                httper.Charset = cbxPageCharset.Text.Trim();
                ShowMessage("");
                ShowMessage("");
                ShowMessage("BEGIN REQUEST:");
                ShowCookie();
                if (ckbxIsPost.Checked)
                {
                    httper.PostData = tbxPostData.Text.Trim();// HttpUtility.UrlEncode("", Encoding.GetEncoding(cbxPageCharset.Text.Trim()));
                    httper.RequestStringAsync(EnumRequestMethod.POST);
                }
                else
                {
                    httper.RequestStringAsync(EnumRequestMethod.GET);
                }
            }
        }

        #region WebBrowser
        void wbMain_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
        void wbMain_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            throw new NotImplementedException();
        }
        void wbMain_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (this.ckbxUserWebBrowser.Checked)
            {
                string[] cookiecontent = this.wbMain.Document.Cookie.Split(';');
                foreach (string str in cookiecontent)
                {
                    string[] cookieNameValue = str.Split('=');
                    Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
                    ck.Domain = this.wbMain.Document.Url.ToString();//必须写对
                    this.cookie.Add(ck);
                }
                //this.cookie.SetCookies(new Uri(cbxUrl.Text.Trim()), this.wbMain.Document.Cookie);
            }
            ShowMessage(this.wbMain.Document.Body.InnerHtml);

            //this.tbxMain.Text += wbMain.Document.Body.GetElementsByTagName("sg_city_food");
            if (this.tbxPostData.Text.Trim() != string.Empty)
            {
                if (this.tbxMain.Text.Trim() == "n")
                {
                    HtmlElementCollection hec = wbMain.Document.Body.GetElementsByTagName("DIV")["body_c"].GetElementsByTagName("span");
                    ShowMessage("============= I Get It ===============");
                    ShowMessage(hec["html_id_current_city_name"].InnerText);
                    for (int i = 0; i < 4; i++)
                    {
                        ShowMessage(string.Format("{0}/{1}", hec[string.Format("r{0}", i)].InnerText, hec[string.Format("d{0}", i)].InnerText));
                    }
                }
                else
                {
                    ShowMessage("============ Get Building ============");
                    HtmlElementCollection hecBuilding = wbMain.Document.Body.GetElementsByTagName("DIV")["city_build_resource"].GetElementsByTagName("div");
                    foreach (HtmlElement he in hecBuilding)
                    {
                        ShowMessage(string.Format("HtmlElementIndex:{0}, TagName:{1}, id:{2}, html:{3}", he.TabIndex, he.TagName, he.Id, he.InnerHtml));
                    }
                }

            }
        }
        private void cbxUrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string date = Utils.Timestamp2Date(tbxPostData.Text.Trim());
            //string aaaa = Utils.UnixTimestamp();
            if (tbxPostData.Text.Trim() != string.Empty)
            {
                IHTMLWindow2 js = (IHTMLWindow2)wbMain.Document.Window.DomWindow;
                string code = string.Format("alert({0})", tbxPostData.Text.Trim());
                try
                {
                    js.execScript(code, "javascript");
                }
                catch
                {
                }

                //string ccc = wbMain.Document.InvokeScript("recource").ToString();
            }
        }
        #endregion
        void httper_RequestStringCompleted(object sender, RequestStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (ckbxUserWebBrowser.Checked)
                {
                    this.wbMain.DocumentText = e.ResponseString;
                }
                ShowMessage(e.ResponseString);
                ShowCookie();
            }
            else
            {
                GetException(e.Error);
            }
        }

        void ShowCookie()
        {
            ShowMessage("\r\n=========COOKIE========\r\n");
            ShowMessage("this.cookie");
            foreach (Cookie c in this.cookie.GetCookies(new Uri(this.httper.Url)))
            {
                this.tbxMain.Text += c.Domain + "=" + c.Name + ":" + c.Value + "\r\n";
            }
            ShowMessage("Httper.Cooke:");
            foreach (Cookie c in this.httper.Cookie.GetCookies(new Uri(this.httper.Url)))
            {
                this.tbxMain.Text += c.Domain + "=" + c.Name + ":" + c.Value + "\r\n";
            }
            ShowMessage("");
            ShowMessage("=========END COOKIE=========");
        }
        private void ShowMessage(string message)
        {
            this.tbxMain.Text += string.Format("{0}\r\n", message);
        }
        private void GetException(Exception ex)
        {
            tbxMain.Text += ex.Message;
        }
    }
}
