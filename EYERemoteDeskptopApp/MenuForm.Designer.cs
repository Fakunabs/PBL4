namespace EYERemoteDeskptopApp
{
    partial class MenuForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnChatroom = new System.Windows.Forms.Button();
            this.btnViewSCR = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::EYERemoteDeskptopApp.Properties.Resources.Eye_icon;
            this.pictureBox1.Location = new System.Drawing.Point(8, 23);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // btnChatroom
            // 
            this.btnChatroom.Location = new System.Drawing.Point(611, 239);
            this.btnChatroom.Name = "btnChatroom";
            this.btnChatroom.Size = new System.Drawing.Size(218, 76);
            this.btnChatroom.TabIndex = 5;
            this.btnChatroom.Text = "Chat room";
            this.btnChatroom.UseVisualStyleBackColor = true;
            this.btnChatroom.Click += new System.EventHandler(this.btnChatroom_Click);
            // 
            // btnViewSCR
            // 
            this.btnViewSCR.Location = new System.Drawing.Point(243, 239);
            this.btnViewSCR.Name = "btnViewSCR";
            this.btnViewSCR.Size = new System.Drawing.Size(229, 76);
            this.btnViewSCR.TabIndex = 4;
            this.btnViewSCR.Text = "view partner screen";
            this.btnViewSCR.UseVisualStyleBackColor = true;
            this.btnViewSCR.Click += new System.EventHandler(this.btnViewSCR_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(472, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "welcome user!";
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1946, 949);
            this.Controls.Add(this.btnChatroom);
            this.Controls.Add(this.btnViewSCR);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MenuForm";
            this.Padding = new System.Windows.Forms.Padding(30, 92, 30, 31);
            this.Text = "  Remote Desktop Application";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MenuForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnChatroom;
        private System.Windows.Forms.Button btnViewSCR;
        private System.Windows.Forms.Label label1;
    }
}

