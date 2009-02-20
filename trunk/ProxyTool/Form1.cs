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
using Natsuhime.Common;

namespace ProxyTool
{
    public partial class Form1 : Form
    {
        string _ProxyListFilePath;
        string _ConfigPath;
        static Dictionary<string, object> _Config;
        ProxySpider ps;
        ProxyValidater pv2;
        public Form1()
        {
            InitializeComponent();
            _ProxyListFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProxyList.dat");
            _ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.dat");

            _Config = (Dictionary<string, object>)Config.LoadConfig(_ConfigPath);
        }

        void ShowMessage(string message)
        {
            this.ShowMessage("", message, "");
        }
        void ShowMessage(string title, string message, string extMessage)
        {
            if (extMessage != string.Empty)
            {
                this.tbxMessage.Text += string.Format("[{0}]{1}({2})", title, message, extMessage);
            }
            else
            {
                this.tbxMessage.Text += string.Format("[{0}]{1}", title, message);
            }
            this.tbxMessage.Text += Environment.NewLine;
            this.tbxMessage.SelectionStart = this.tbxMessage.TextLength;
            this.tbxMessage.ScrollToCaret();
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
            List<ProxySourcePageInfo> pspi = (List<ProxySourcePageInfo>)_Config["source_pageurl"];
            
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
            List<ProxyInfo> list = (List<ProxyInfo>)SerializationHelper.LoadJson(_ProxyListFilePath, typeof(List<ProxyInfo>));
            if (list != null && list.Count > 0)
            {
                pv2 = new ProxyValidater((ProxyValidateUrlInfo)_Config["validate_pageurl"]);
                pv2.Completed += new CompletedEventHandler(pv2_ValidateCompleted);
                pv2.StatusChanged += new StatusChangedEventHandler(pv2_StatusChanged);
                pv2.ValidateAsync(ref list, 0);
            }
            else
            {
                ShowMessage("验证列表", "待验证列表为空,已退出!", "");
            }
        }
        void pv2_StatusChanged(Natsuhime.Events.MessageEventArgs e)
        {
            ShowMessage(e.UserState.ToString(), e.Message, e.ExtMessage);
        }
        void pv2_ValidateCompleted(object sender, CompletedEventArgs e)
        {
            if (e.Error == null)
            {
                ShowMessage("验证列表", "完成.", "");
                SerializationHelper.SaveJson(RemoveExitsProxy(e.ProxyList), _ProxyListFilePath);
                ShowMessage("验证列表", "保存到配置成功.", "");
            }
            else
            {
                //TODO
                ShowMessage("错误", e.Error.Message, "");
            }
        }
        #endregion
    }
}
