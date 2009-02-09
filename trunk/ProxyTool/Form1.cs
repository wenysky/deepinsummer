using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Natsuhime.Proxy;

namespace ProxyTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            ps.BeginFetch();
        }

        void ps_Completed(object sender, Natsuhime.Events.ReturnCompletedEventArgs e)
        {
            List<ProxyInfo> list = (List<ProxyInfo>)e.ReturnObject;
        }
    }
}
