using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cy_Connection_Client;

namespace Clients
{
    public partial class Login : Form
    {
        public string username = "";
        GroupList instance;
        public bool loginSuccess = false;
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            username.Replace(" ", "_");
            loginSuccess = true;
            this.username = username;
            this.DialogResult = DialogResult.OK;
            //this.Close();
        }
        public void GroupListReference(GroupList groupList)
        {
            instance = groupList;
        }
        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Application.Exit();
        }
    }
}
