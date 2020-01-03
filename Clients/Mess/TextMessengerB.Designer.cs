namespace Clients.Mess
{
    partial class TextMessengerB
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
            this.lbsender = new System.Windows.Forms.Label();
            this.tbMess = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbsender
            // 
            this.lbsender.Location = new System.Drawing.Point(435, 0);
            this.lbsender.Name = "lbsender";
            this.lbsender.Size = new System.Drawing.Size(57, 19);
            this.lbsender.TabIndex = 1;
            this.lbsender.Text = "label1";
            this.lbsender.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbMess
            // 
            this.tbMess.BackColor = System.Drawing.SystemColors.Control;
            this.tbMess.Location = new System.Drawing.Point(167, 19);
            this.tbMess.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tbMess.Name = "tbMess";
            this.tbMess.Size = new System.Drawing.Size(325, 45);
            this.tbMess.TabIndex = 0;
            this.tbMess.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextMessengerB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tbMess);
            this.Controls.Add(this.lbsender);
            this.Name = "TextMessengerB";
            this.Size = new System.Drawing.Size(500, 80);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbsender;
        private System.Windows.Forms.Label tbMess;
    }
}
