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
    public partial class TextMessengerB : UserControl
    {
        public TextMessengerB(string sendername,string Mess)
        {
            InitializeComponent();
            lbsender.Text = sendername;
            tbMess.Text = Mess;
        }
    }
}
