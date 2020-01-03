using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clients.Mess
{
    public partial class FileMessengerB : UserControl
    {
        public delegate void DownloadDelegate(int id,string name);
        public event DownloadDelegate DownLoadfileRequest;
        public int ID;
        public string name;
      
        public FileMessengerB(string sender,int id, string name)
        {
            InitializeComponent();
            this.ID = id;
            this.name = name;
            lbFile.Text = name;
            lbSender.Text = sender;
        }

        private void lbFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (DownLoadfileRequest != null)
            {
                DownLoadfileRequest(ID,name);
            }
         
        }
    }
}
