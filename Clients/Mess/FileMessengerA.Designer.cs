namespace Clients.Mess
{
    partial class FileMessengerA
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbFile = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel1.Controls.Add(this.lbFile);
            this.panel1.Location = new System.Drawing.Point(4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(215, 24);
            this.panel1.TabIndex = 0;
            // 
            // lbFile
            // 
            this.lbFile.AutoSize = true;
            this.lbFile.LinkColor = System.Drawing.Color.Black;
            this.lbFile.Location = new System.Drawing.Point(4, 6);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(55, 13);
            this.lbFile.TabIndex = 0;
            this.lbFile.TabStop = true;
            this.lbFile.Text = "linkLabel1";
            this.lbFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbFile_LinkClicked);
            // 
            // FileMessengerA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Name = "FileMessengerA";
            this.Size = new System.Drawing.Size(500, 36);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel lbFile;
    }
}
