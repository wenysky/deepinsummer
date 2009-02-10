using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Natsuhime.Proxy;
using Newtonsoft.Json;
using System.IO;

namespace ProxyTool
{
    public partial class Form1 : Form
    {
        string _ProxyListFilePath;
        public Form1()
        {
            InitializeComponent();
            _ProxyListFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProxyList.dat");
        }

        void ShowMessage(string title, string message, string extMessage)
        {
            if (extMessage != string.Empty)
            {
                this.tbxMessage.Text += string.Format("[{0}]{1}({2})\r\n", title, message, extMessage);
            }
            else
            {
                this.tbxMessage.Text += string.Format("[{0}]{1}\r\n", title, message);
            }
        }
        List<ProxyInfo> RemoveExitsProxy(List<ProxyInfo> list)
        {
            Dictionary<string, ProxyInfo> dic = new Dictionary<string, ProxyInfo>();
            foreach (ProxyInfo info in list)
            {
                string key = info.Address + ":" + info.Port;
                if (!dic.ContainsKey(key))
                {
                    dic.Add(key, info);
                }
            }

            List<ProxyInfo> newList = new List<ProxyInfo>();
            foreach (ProxyInfo info in dic.Values)
            {
                newList.Add(info);
            }
            return newList;
        }

        void ps_StatusChanged(object sender, Natsuhime.Events.MessageEventArgs e)
        {
            ShowMessage(e.Title, e.Message, e.ExtMessage);
        }
        void ps_Completed(object sender, Natsuhime.Events.ReturnCompletedEventArgs e)
        {
            List<ProxyInfo> list = (List<ProxyInfo>)e.ReturnObject;

            string oldJson = File.ReadAllText(_ProxyListFilePath, new UTF8Encoding(true, true));
            if (oldJson.Trim() != string.Empty)
            {
                List<ProxyInfo> oldList = (List<ProxyInfo>)JavaScriptConvert.DeserializeObject(oldJson, typeof(List<ProxyInfo>));
                list.AddRange(oldList);
            }

            string json = JavaScriptConvert.SerializeObject(RemoveExitsProxy(list));
            File.WriteAllText(_ProxyListFilePath, json, new UTF8Encoding(true, true));
        }

        private void btnGetProxyList_Click(object sender, EventArgs e)
        {
            List<ProxySourcePageInfo> pspi = new List<ProxySourcePageInfo>();
            ProxySourcePageInfo info = new ProxySourcePageInfo();
            info.PageUrl = "http://www.proxycn.com/html_proxy/30fastproxy-1.html";
            info.RegexString = @"clip\(\'(.*)\'\);alert";
            info.PageCharset = "gb2312";
            pspi.Add(info);
            ProxySpider ps = new ProxySpider(pspi);
            ps.Completed += new EventHandler<Natsuhime.Events.ReturnCompletedEventArgs>(ps_Completed);
            ps.StatusChanged += new EventHandler<Natsuhime.Events.MessageEventArgs>(ps_StatusChanged);
            ps.BeginFetch();
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            string oldJson = File.ReadAllText(_ProxyListFilePath, new UTF8Encoding(true, true));
            if (oldJson.Trim() != string.Empty)
            {
                List<ProxyInfo> list = (List<ProxyInfo>)JavaScriptConvert.DeserializeObject(oldJson, typeof(List<ProxyInfo>));

                ProxyValidater pv = new ProxyValidater();
                pv.Completed += new EventHandler<Natsuhime.Events.ReturnCompletedEventArgs>(pv_Completed);
                pv.StatusChanged += new EventHandler<Natsuhime.Events.MessageEventArgs>(pv_StatusChanged);
                pv.BeginValidate(list);
            }
        }

        void pv_StatusChanged(object sender, Natsuhime.Events.MessageEventArgs e)
        {
            ShowMessage(e.Title, e.Message, e.ExtMessage);
        }

        void pv_Completed(object sender, Natsuhime.Events.ReturnCompletedEventArgs e)
        {
            List<ProxyInfo> list = (List<ProxyInfo>)e.ReturnObject;
            string json = JavaScriptConvert.SerializeObject(RemoveExitsProxy(list));
            File.WriteAllText(_ProxyListFilePath, json, new UTF8Encoding(true, true));
        }
    }
}
