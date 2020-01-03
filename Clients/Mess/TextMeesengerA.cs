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
    public partial class TextMeesengerA : UserControl
    {
        public TextMeesengerA(string Mess)
        {
            InitializeComponent();
            this.tbMess.Text = Mess;
        }


    }
}
