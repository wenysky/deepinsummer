namespace HttpUtilityTool
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ckbxIsPost = new System.Windows.Forms.CheckBox();
            this.ckbxUserWebBrowser = new System.Windows.Forms.CheckBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.cbxPageCharset = new System.Windows.Forms.ComboBox();
            this.ckbxKeepCookie = new System.Windows.Forms.CheckBox();
            this.cbxUrl = new System.Windows.Forms.ComboBox();
            this.tbxPostData = new System.Windows.Forms.TextBox();
            this.tcMessage = new System.Windows.Forms.TabControl();
            this.tpHtml = new System.Windows.Forms.TabPage();
            this.wbMain = new System.Windows.Forms.WebBrowser();
            this.tpText = new System.Windows.Forms.TabPage();
            this.tbxMain = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tcMessage.SuspendLayout();
            this.tpHtml.SuspendLayout();
            this.tpText.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbxPostData, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tcMessage, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(690, 446);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ckbxIsPost);
            this.panel1.Controls.Add(this.ckbxUserWebBrowser);
            this.panel1.Controls.Add(this.btnSend);
            this.panel1.Controls.Add(this.cbxPageCharset);
            this.panel1.Controls.Add(this.ckbxKeepCookie);
            this.panel1.Controls.Add(this.cbxUrl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(684, 26);
            this.panel1.TabIndex = 0;
            // 
            // ckbxIsPost
            // 
            this.ckbxIsPost.AutoSize = true;
            this.ckbxIsPost.Location = new System.Drawing.Point(396, 7);
            this.ckbxIsPost.Name = "ckbxIsPost";
            this.ckbxIsPost.Size = new System.Drawing.Size(48, 16);
            this.ckbxIsPost.TabIndex = 5;
            this.ckbxIsPost.Text = "Post";
            this.ckbxIsPost.UseVisualStyleBackColor = true;
            // 
            // ckbxUserWebBrowser
            // 
            this.ckbxUserWebBrowser.AutoSize = true;
            this.ckbxUserWebBrowser.Location = new System.Drawing.Point(516, 5);
            this.ckbxUserWebBrowser.Name = "ckbxUserWebBrowser";
            this.ckbxUserWebBrowser.Size = new System.Drawing.Size(84, 16);
            this.ckbxUserWebBrowser.TabIndex = 4;
            this.ckbxUserWebBrowser.Text = "UseBrowser";
            this.ckbxUserWebBrowser.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(606, 1);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // cbxPageCharset
            // 
            this.cbxPageCharset.FormattingEnabled = true;
            this.cbxPageCharset.Items.AddRange(new object[] {
            "GBK",
            "GB2312",
            "UTF-8",
            "BIG5"});
            this.cbxPageCharset.Location = new System.Drawing.Point(344, 3);
            this.cbxPageCharset.Name = "cbxPageCharset";
            this.cbxPageCharset.Size = new System.Drawing.Size(46, 20);
            this.cbxPageCharset.TabIndex = 2;
            // 
            // ckbxKeepCookie
            // 
            this.ckbxKeepCookie.AutoSize = true;
            this.ckbxKeepCookie.Checked = true;
            this.ckbxKeepCookie.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbxKeepCookie.Location = new System.Drawing.Point(450, 7);
            this.ckbxKeepCookie.Name = "ckbxKeepCookie";
            this.ckbxKeepCookie.Size = new System.Drawing.Size(60, 16);
            this.ckbxKeepCookie.TabIndex = 1;
            this.ckbxKeepCookie.Text = "Cookie";
            this.ckbxKeepCookie.UseVisualStyleBackColor = true;
            // 
            // cbxUrl
            // 
            this.cbxUrl.FormattingEnabled = true;
            this.cbxUrl.Items.AddRange(new object[] {
            "http://50sg2.yeswan.com/main.php",
            "http://50sg2.yeswan.com/main.php#city_build_resource"});
            this.cbxUrl.Location = new System.Drawing.Point(3, 3);
            this.cbxUrl.Name = "cbxUrl";
            this.cbxUrl.Size = new System.Drawing.Size(335, 20);
            this.cbxUrl.TabIndex = 0;
            this.cbxUrl.SelectedIndexChanged += new System.EventHandler(this.cbxUrl_SelectedIndexChanged);
            // 
            // tbxPostData
            // 
            this.tbxPostData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxPostData.Location = new System.Drawing.Point(3, 35);
            this.tbxPostData.Multiline = true;
            this.tbxPostData.Name = "tbxPostData";
            this.tbxPostData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxPostData.Size = new System.Drawing.Size(684, 64);
            this.tbxPostData.TabIndex = 4;
            // 
            // tcMessage
            // 
            this.tcMessage.Controls.Add(this.tpHtml);
            this.tcMessage.Controls.Add(this.tpText);
            this.tcMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMessage.Location = new System.Drawing.Point(3, 105);
            this.tcMessage.Name = "tcMessage";
            this.tcMessage.SelectedIndex = 0;
            this.tcMessage.Size = new System.Drawing.Size(684, 338);
            this.tcMessage.TabIndex = 5;
            // 
            // tpHtml
            // 
            this.tpHtml.Controls.Add(this.wbMain);
            this.tpHtml.Location = new System.Drawing.Point(4, 22);
            this.tpHtml.Name = "tpHtml";
            this.tpHtml.Padding = new System.Windows.Forms.Padding(3);
            this.tpHtml.Size = new System.Drawing.Size(676, 312);
            this.tpHtml.TabIndex = 0;
            this.tpHtml.Text = "网页";
            this.tpHtml.UseVisualStyleBackColor = true;
            // 
            // wbMain
            // 
            this.wbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbMain.Location = new System.Drawing.Point(3, 3);
            this.wbMain.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbMain.Name = "wbMain";
            this.wbMain.Size = new System.Drawing.Size(670, 306);
            this.wbMain.TabIndex = 0;
            // 
            // tpText
            // 
            this.tpText.Controls.Add(this.tbxMain);
            this.tpText.Location = new System.Drawing.Point(4, 22);
            this.tpText.Name = "tpText";
            this.tpText.Padding = new System.Windows.Forms.Padding(3);
            this.tpText.Size = new System.Drawing.Size(676, 312);
            this.tpText.TabIndex = 1;
            this.tpText.Text = "内容";
            this.tpText.UseVisualStyleBackColor = true;
            // 
            // tbxMain
            // 
            this.tbxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxMain.Location = new System.Drawing.Point(3, 3);
            this.tbxMain.Multiline = true;
            this.tbxMain.Name = "tbxMain";
            this.tbxMain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxMain.Size = new System.Drawing.Size(670, 306);
            this.tbxMain.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 446);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "HttpRequestTool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tcMessage.ResumeLayout(false);
            this.tpHtml.ResumeLayout(false);
            this.tpText.ResumeLayout(false);
            this.tpText.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox ckbxKeepCookie;
        private System.Windows.Forms.ComboBox cbxUrl;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ComboBox cbxPageCharset;
        private System.Windows.Forms.TextBox tbxPostData;
        private System.Windows.Forms.CheckBox ckbxUserWebBrowser;
        private System.Windows.Forms.TabControl tcMessage;
        private System.Windows.Forms.TabPage tpHtml;
        private System.Windows.Forms.TabPage tpText;
        private System.Windows.Forms.WebBrowser wbMain;
        private System.Windows.Forms.TextBox tbxMain;
        private System.Windows.Forms.CheckBox ckbxIsPost;
    }
}

