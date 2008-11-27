using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Natsuhime;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void ShowMessage(string message)
        {
            this.tbxMessage.Text += string.Format("{0}\r\n", message);
        }

        private void btnSelectTemplateFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            System.Diagnostics.Debug.Write(Environment.GetEnvironmentVariables());

            //fbd.RootFolder
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbxTemplateFolder.Text = fbd.SelectedPath;
            }
        }

        private void btnSelectPagefileFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbxPagefileFolder.Text = fbd.SelectedPath;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            NewTemplate nt = new NewTemplate("xxoo");
            nt.CreateFromFolder(tbxTemplateFolder.Text, tbxPagefileFolder.Text);

        }

        private void btnOldVersion_Click(object sender, EventArgs e)
        {
        }
    }
}
