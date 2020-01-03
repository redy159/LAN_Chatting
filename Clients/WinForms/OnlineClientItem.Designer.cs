namespace Clients.WinForms
{
    partial class OnlineClientItem
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
            this.lbName = new System.Windows.Forms.Label();
            this.btCreat = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbName
            // 
            this.lbName.BackColor = System.Drawing.Color.White;
            this.lbName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbName.Location = new System.Drawing.Point(110, 9);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(109, 21);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "label1";
            this.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbName.Click += new System.EventHandler(this.label1_Click);
            // 
            // btCreat
            // 
            this.btCreat.BackColor = System.Drawing.Color.LightGray;
            this.btCreat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btCreat.Location = new System.Drawing.Point(231, 1);
            this.btCreat.Name = "btCreat";
            this.btCreat.Size = new System.Drawing.Size(139, 37);
            this.btCreat.TabIndex = 1;
            this.btCreat.Text = "Tao phong chat";
            this.btCreat.UseVisualStyleBackColor = false;
            this.btCreat.Click += new System.EventHandler(this.btCreat_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Username";
            // 
            // OnlineClientItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btCreat);
            this.Controls.Add(this.lbName);
            this.Name = "OnlineClientItem";
            this.Size = new System.Drawing.Size(440, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Button btCreat;
        private System.Windows.Forms.Label label2;
    }
}
