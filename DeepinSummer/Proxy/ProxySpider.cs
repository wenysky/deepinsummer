using System;
using System.Text;
using System.Collections.Generic;

using Natsuhime;

namespace Natsuhime.Proxy
{
    public class ProxySpider
    {
        NewHttper _httper;
        List<ProxySourcePageInfo> _SourcePageInfo;
        List<ProxyInfo> _ProxyList;
        int _CompletedCount;
        public ProxySpider(List<ProxySourcePageInfo> sourcePageInfo)
        {
            this._SourcePageInfo = sourcePageInfo;
            this._ProxyList = new List<ProxyInfo>();
            this._httper = new NewHttper();
            this._httper.RequestStringCompleted += new NewHttper.RequestStringCompleteEventHandler(_httper_RequestStringCompleted);
        }

        void _httper_RequestStringCompleted(object sender, RequestStringCompletedEventArgs e)
        {
            this._CompletedCount++;
            if (e.Error == null)
            {
                System.Text.RegularExpressions.MatchCollection mc = RegexUtility.GetMatchFull(e.ResponseString, e.UserState.ToString());
                if (mc != null)
                {
                    foreach (System.Text.RegularExpressions.Match m in mc)
                    {
                        ProxyInfo info = new ProxyInfo();
                        info.Name = "";
                        info.Address = m.Groups[1].Value.Split(':')[0];
                        info.Port = Convert.ToInt32(m.Groups[1].Value.Split(':')[1]);
                        SendStatusChanged(string.Format("获得地址:{0}:{1}", info.Address, info.Port), "");
                        this._ProxyList.Add(info);
                    }
                }
            }
            else
            {
#warning todo error
            }
            if (this._CompletedCount == this._SourcePageInfo.Count)
            {
                if (this.Completed != null)
                {
                    this.Completed(this, new Natsuhime.Events.ReturnCompletedEventArgs(this._ProxyList));
                }
            }
        }

        public void BeginFetch()
        {
            foreach (ProxySourcePageInfo pspi in this._SourcePageInfo)
            {
                SendStatusChanged(string.Format("Url：{0}分析开始", pspi.PageUrl), "");
                this._httper.Timeout = 10000;
                this._httper.Charset = pspi.PageCharset;
                this._httper.Url = pspi.PageUrl;
                this._httper.RequestStringAsync(EnumRequestMethod.GET, pspi.RegexString);

                //RegexFunc rFunc = new RegexFunc();
                //System.Text.RegularExpressions.MatchCollection m = rFunc.GetMatchFull(returnData, objProxyGetInfo.Regex);
                //List<ProxyInfo> objProxyList = new List<ProxyInfo>();
                //foreach (System.Text.RegularExpressions.Match objMatch in m)
                //{
                //    ProxyInfo objProxyInfo = new ProxyInfo();
                //    objProxyInfo.Address = objMatch.Groups[1].Value.Split(':')[0];
                //    objProxyInfo.Port = int.Parse(objMatch.Groups[1].Value.Split(':')[1]);
                //    objProxyInfo.Name = "暂无";
                //    objProxyList.Add(objProxyInfo);
                //    tbMessage.Text += string.Format("获得地址:{0}:{1}\r\n", objProxyInfo.Address, objProxyInfo.Port);
                //}

                // Utility.Utility.WriteProxyListToXml(objProxyList);
            }
        }

        void SendStatusChanged(string message, string extMessage)
        {
            if (this.StatusChanged != null)
            {
            }
        }

        public event EventHandler<Events.MessageEventArgs> StatusChanged;
        public event EventHandler<Events.ProgressChangedEventArgs> ProgressChanged;
        public event EventHandler<Events.ReturnCompletedEventArgs> Completed;
        public event EventHandler<Events.CommonErrorEventArgs> Errored;
    }
}
