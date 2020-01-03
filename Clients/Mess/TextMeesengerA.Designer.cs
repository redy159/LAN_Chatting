namespace Clients.Mess
{
    partial class TextMeesengerA
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
            this.tbMess = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbMess
            // 
            this.tbMess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tbMess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbMess.Location = new System.Drawing.Point(10, 8);
            this.tbMess.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tbMess.Name = "tbMess";
            this.tbMess.Size = new System.Drawing.Size(325, 45);
            this.tbMess.TabIndex = 0;
            this.tbMess.Text = "asdasda\'";
            // 
            // TextMeesengerA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tbMess);
            this.Name = "TextMeesengerA";
            this.Size = new System.Drawing.Size(500, 63);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label tbMess;
    }
}
