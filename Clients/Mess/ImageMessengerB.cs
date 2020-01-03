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
    public partial class ImageMessengerB : UserControl
    {
        private object image;

        public ImageMessengerB(string sender,object img)
        {
            InitializeComponent();
            lbsender.Text = sender;
            this.image = img;
            putImage();
        }
        private void putImage()
        {
            // o thể sửa UI từ 1 thread khác
            // https://stackoverflow.com/questions/14750872/c-sharp-controls-created-on-one-thread-cannot-be-parented-to-a-control-on-a-diff
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    Image img = image as Image;
                    pictureBox.Image = img;
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                });
            }
            else
            {
                Image img = image as Image;
                pictureBox.Image = img;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }

        }

        private void pictureBox_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            Form form = new Form();

            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.Image = image as Image;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            form.Controls.Add(pictureBox);
            form.WindowState = FormWindowState.Maximized;
            form.ShowDialog();
        }
    }
}
