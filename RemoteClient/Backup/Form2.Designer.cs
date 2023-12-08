namespace RemoteClient
{
    partial class Form2
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
            this.theImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.theImage)).BeginInit();
            this.SuspendLayout();
            // 
            // theImage
            // 
            this.theImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.theImage.Location = new System.Drawing.Point(0, 0);
            this.theImage.Margin = new System.Windows.Forms.Padding(0);
            this.theImage.Name = "theImage";
            this.theImage.Size = new System.Drawing.Size(284, 264);
            this.theImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.theImage.TabIndex = 0;
            this.theImage.TabStop = false;
            this.theImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.theImage_MouseMove);
            this.theImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.theImage_MouseClick);
            this.theImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.theImage_MouseDown);
            this.theImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.theImage_MouseUp);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.theImage);
            this.KeyPreview = true;
            this.Name = "Form2";
            this.Text = "DT Remote Client";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form2_FormClosed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form2_KeyUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseMove);
            this.ResizeEnd += new System.EventHandler(this.Form2_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.theImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox theImage;
    }
}