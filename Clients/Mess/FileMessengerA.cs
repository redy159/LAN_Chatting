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
    public partial class FileMessengerA : UserControl
    {
        public delegate void  DownloadDelegate(string dir);
        public event DownloadDelegate OpenfileEvent;
        public string directory;
        public string name; 
        public FileMessengerA(string dir, string name)
        {
            InitializeComponent();
            lbFile.Text = name;
            this.directory = dir;
            this.name = name;

        }

        private void lbFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (OpenfileEvent != null)
            {
                OpenfileEvent(this.directory);

            }
        }
    }
}
