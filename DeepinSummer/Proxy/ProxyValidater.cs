using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Natsuhime.Proxy
{
    public class ProxyValidater
    {
        List<ProxyInfo> _ProxyList;
        List<ProxyInfo> _ProxyListOK;
        public void BeginValidate(List<ProxyInfo> proxyList)
        {
            _ProxyList = proxyList;
            _ProxyListOK = new List<ProxyInfo>();

            const int ThreadNum = 2;
            //每个执行方法线程一个Event,最后数组中所有Event都OK后,表示完成
            ManualResetEvent[] doneEvents = new ManualResetEvent[ThreadNum];
            //用线程池开始线程
            for (int i = 0; i < ThreadNum; i++)
            {
                doneEvents[i] = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(Validate, doneEvents[i]);
            }
            //等待直到Event标识全部OK
            WaitHandle.WaitAll(doneEvents);

            if (this.Completed != null)
            {
                this.Completed(this, new Natsuhime.Events.ReturnCompletedEventArgs(_ProxyListOK));
            }
        }

        private void Validate(object mrevent)
        {
            ManualResetEvent doneEvent = (ManualResetEvent)mrevent;
            Monitor.Enter(_ProxyList);
            while (_ProxyList.Count > 0)
            {
                ProxyInfo info = _ProxyList[0];
                _ProxyList.Remove(info);
                Monitor.Exit(_ProxyList);

                NewHttper httper = new NewHttper();
                httper.Proxy = new System.Net.WebProxy(info.Address, info.Port);
                httper.Url = "http://www.ip138.com/ips.asp";
                httper.Charset = "gb2312";
                SendStatusChanged(string.Format("{0}：{1}校验中...", info.Address, info.Port), "");
                string returnData;
                try
                {
                    returnData = httper.HttpGet();
                }
                catch (Exception ex)
                {
                    returnData = string.Empty;
                }
                if (returnData != string.Empty && "124.207.144.194" != RegexUtility.GetMatch(returnData, "您的IP地址是：\\[(.*)\\] 来自\\："))
                {
                    Monitor.Enter(_ProxyListOK);
                    _ProxyListOK.Add(info);
                    Monitor.Exit(_ProxyListOK);
                    SendStatusChanged(string.Format("[成功]{0}：{1}", info.Address, info.Port), "");
                }
                else
                {
                    SendStatusChanged(string.Format("[失败]{0}：{1}", info.Address, info.Port), "");
                }
            }
            Monitor.Exit(_ProxyList);
            doneEvent.Set();
        }

        void SendStatusChanged(string message, string extMessage)
        {
            if (this.StatusChanged != null)
            {
                this.StatusChanged(this, new Natsuhime.Events.MessageEventArgs("验证地址", message, extMessage));
            }
        }

        public event EventHandler<Events.MessageEventArgs> StatusChanged;
        public event EventHandler<Events.ProgressChangedEventArgs> ProgressChanged;
        public event EventHandler<Events.ReturnCompletedEventArgs> Completed;
        public event EventHandler<Events.CommonErrorEventArgs> Errored;
    }
}
