using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cy_Connection_Sever;

namespace Sever
{
    public partial class sever : Form
    {
        Sever_module myserver;
        System.Timers.Timer pingtimer;
        public sever()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void sever_Load(object sender, EventArgs e)
        {
            myserver = new Sever_module();
            myserver.Connect();
            myserver.Ping();
            pingtimer = new System.Timers.Timer();
            pingtimer.Interval = 1500;
            pingtimer.Elapsed += Pingtimer_Tick;
            pingtimer.AutoReset = true;
            pingtimer.Enabled = true;
        }

        private void Pingtimer_Tick(object sender, EventArgs e)
        {
            // bỏ comment nó sẽ tự ping 
            myserver.Ping();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            myserver.Ping();
        }
    }
}
