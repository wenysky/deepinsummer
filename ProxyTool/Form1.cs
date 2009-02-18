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
        ProxySpider ps;
        ProxyValidater pv2;
        public Form1()
        {
            InitializeComponent();
            _ProxyListFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProxyList.dat");
        }

        void ShowMessage(string message)
        {
            this.ShowMessage("", message, "");
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

        #region 整理
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
        #endregion

        #region 获取
        private void btnGetProxyList_Click(object sender, EventArgs e)
        {
            List<ProxySourcePageInfo> pspi = new List<ProxySourcePageInfo>();
            ProxySourcePageInfo info = new ProxySourcePageInfo();
            info.PageUrl = "http://www.proxycn.com/html_proxy/30fastproxy-1.html";
            info.RegexString = @"clip\(\'(.*)\'\);alert";
            info.PageCharset = "gb2312";
            pspi.Add(info);
            ps = new ProxySpider(pspi);
            ps.Completed += new EventHandler<Natsuhime.Events.ReturnCompletedEventArgs>(ps_Completed);
            ps.StatusChanged += new EventHandler<Natsuhime.Events.MessageEventArgs>(ps_StatusChanged);
            ps.BeginFetch();
        }
        void ps_StatusChanged(object sender, Natsuhime.Events.MessageEventArgs e)
        {
            ShowMessage(e.Title, e.Message, e.ExtMessage);
        }
        void ps_Completed(object sender, Natsuhime.Events.ReturnCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                ShowMessage("获取列表", "完成.", "");
                List<ProxyInfo> list = (List<ProxyInfo>)e.ReturnObject;

                string oldJson = File.ReadAllText(_ProxyListFilePath, new UTF8Encoding(true, true));
                if (oldJson.Trim() != string.Empty)
                {
                    List<ProxyInfo> oldList = (List<ProxyInfo>)JavaScriptConvert.DeserializeObject(oldJson, typeof(List<ProxyInfo>));
                    list.AddRange(oldList);
                }

                string json = JavaScriptConvert.SerializeObject(RemoveExitsProxy(list));
                File.WriteAllText(_ProxyListFilePath, json, new UTF8Encoding(true, true));
                ShowMessage("获取列表", "保存配置成功.", "");
            }
            else
            {
                //TODO
            }
        }
        #endregion

        #region 验证
        private void btnValidate_Click(object sender, EventArgs e)
        {
            string oldJson = File.ReadAllText(_ProxyListFilePath, new UTF8Encoding(true, true));
            if (oldJson.Trim() != string.Empty)
            {
                List<ProxyInfo> list = (List<ProxyInfo>)JavaScriptConvert.DeserializeObject(oldJson, typeof(List<ProxyInfo>));
                pv2 = new ProxyValidater();
                pv2.CalculatePrimeCompleted += new CalculatePrimeCompletedEventHandler(pv2_CalculatePrimeCompleted);
                pv2.StatusChanged += new StatusChangedEventHandler(pv2_StatusChanged);
                pv2.ValidateAsync(ref list, 0);
            }
        }
        void pv2_StatusChanged(Natsuhime.Events.MessageEventArgs e)
        {
            ShowMessage(e.UserState.ToString(), e.Message, e.ExtMessage);
        }
        void pv2_CalculatePrimeCompleted(object sender, CalculatePrimeCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                ShowMessage("验证列表", "完成.", "");
                string json = JavaScriptConvert.SerializeObject(RemoveExitsProxy(e.ProxyList));
                File.WriteAllText(_ProxyListFilePath, json, new UTF8Encoding(true, true));
                ShowMessage("验证列表", "保存到配置成功.", "");
            }
            else
            {
                //TODO
            }
        }
        #endregion
    }
}
