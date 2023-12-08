namespace RemoteClient
{
    partial class FrmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.CmdHelp2 = new System.Windows.Forms.Button();
            this.ChkLeftMenuConnect = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.CmdConnect = new System.Windows.Forms.Button();
            this.ChkBlackWallpaper = new System.Windows.Forms.CheckBox();
            this.TxtPort = new System.Windows.Forms.TextBox();
            this.TxtIPAddress = new System.Windows.Forms.TextBox();
            this.GroupConnect = new System.Windows.Forms.GroupBox();
            this.CmdFirewall = new System.Windows.Forms.Button();
            this.ImgDefaultBackground = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.theImage = new System.Windows.Forms.PictureBox();
            this.GroupConnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImgDefaultBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.theImage)).BeginInit();
            this.SuspendLayout();
            // 
            // CmdHelp2
            // 
            this.CmdHelp2.Location = new System.Drawing.Point(138, 179);
            this.CmdHelp2.Name = "CmdHelp2";
            this.CmdHelp2.Size = new System.Drawing.Size(91, 30);
            this.CmdHelp2.TabIndex = 58;
            this.CmdHelp2.Text = "Help";
            this.toolTip1.SetToolTip(this.CmdHelp2, "Shut down remote server");
            this.CmdHelp2.UseVisualStyleBackColor = true;
            this.CmdHelp2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CmdHelp_MouseClick);
            // 
            // ChkLeftMenuConnect
            // 
            this.ChkLeftMenuConnect.AutoSize = true;
            this.ChkLeftMenuConnect.BackColor = System.Drawing.Color.Transparent;
            this.ChkLeftMenuConnect.Location = new System.Drawing.Point(44, 132);
            this.ChkLeftMenuConnect.Name = "ChkLeftMenuConnect";
            this.ChkLeftMenuConnect.Size = new System.Drawing.Size(86, 20);
            this.ChkLeftMenuConnect.TabIndex = 57;
            this.ChkLeftMenuConnect.Text = "Left Menu";
            this.toolTip1.SetToolTip(this.ChkLeftMenuConnect, "Scale the remote desktop image down");
            this.ChkLeftMenuConnect.UseVisualStyleBackColor = false;
            this.ChkLeftMenuConnect.CheckedChanged += new System.EventHandler(this.ChkLeftMenuConnect_CheckedChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.Silver;
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.Red;
            this.txtPassword.Location = new System.Drawing.Point(149, 90);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(173, 27);
            this.txtPassword.TabIndex = 48;
            this.txtPassword.Text = "letmein";
            this.toolTip1.SetToolTip(this.txtPassword, "Remote desktop IP-Address");
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // CmdConnect
            // 
            this.CmdConnect.Location = new System.Drawing.Point(36, 179);
            this.CmdConnect.Name = "CmdConnect";
            this.CmdConnect.Size = new System.Drawing.Size(96, 30);
            this.CmdConnect.TabIndex = 46;
            this.CmdConnect.Text = "Connect";
            this.toolTip1.SetToolTip(this.CmdConnect, "Connect to the remote desktop");
            this.CmdConnect.UseVisualStyleBackColor = true;
            this.CmdConnect.Click += new System.EventHandler(this.CmdConnect_Click);
            this.CmdConnect.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CmdConnect_MouseClick);
            // 
            // ChkBlackWallpaper
            // 
            this.ChkBlackWallpaper.AutoSize = true;
            this.ChkBlackWallpaper.BackColor = System.Drawing.Color.Transparent;
            this.ChkBlackWallpaper.Checked = true;
            this.ChkBlackWallpaper.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkBlackWallpaper.Location = new System.Drawing.Point(149, 132);
            this.ChkBlackWallpaper.Name = "ChkBlackWallpaper";
            this.ChkBlackWallpaper.Size = new System.Drawing.Size(129, 20);
            this.ChkBlackWallpaper.TabIndex = 45;
            this.ChkBlackWallpaper.Text = "Black Wallpaper";
            this.toolTip1.SetToolTip(this.ChkBlackWallpaper, "Use black remote desktop");
            this.ChkBlackWallpaper.UseVisualStyleBackColor = false;
            this.ChkBlackWallpaper.CheckedChanged += new System.EventHandler(this.ChkBlackWallpaper_CheckedChanged_1);
            // 
            // TxtPort
            // 
            this.TxtPort.BackColor = System.Drawing.Color.Silver;
            this.TxtPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtPort.ForeColor = System.Drawing.Color.Red;
            this.TxtPort.Location = new System.Drawing.Point(149, 55);
            this.TxtPort.Margin = new System.Windows.Forms.Padding(4);
            this.TxtPort.Name = "TxtPort";
            this.TxtPort.Size = new System.Drawing.Size(173, 27);
            this.TxtPort.TabIndex = 7;
            this.TxtPort.Text = "4000";
            this.toolTip1.SetToolTip(this.TxtPort, "Remote desktop port");
            this.TxtPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtPort_KeyDown);
            // 
            // TxtIPAddress
            // 
            this.TxtIPAddress.BackColor = System.Drawing.Color.Silver;
            this.TxtIPAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtIPAddress.ForeColor = System.Drawing.Color.Red;
            this.TxtIPAddress.Location = new System.Drawing.Point(149, 25);
            this.TxtIPAddress.Margin = new System.Windows.Forms.Padding(4);
            this.TxtIPAddress.Name = "TxtIPAddress";
            this.TxtIPAddress.Size = new System.Drawing.Size(173, 27);
            this.TxtIPAddress.TabIndex = 6;
            this.TxtIPAddress.Text = "10.10.10.25";
            this.toolTip1.SetToolTip(this.TxtIPAddress, "Remote desktop IP-Address");
            this.TxtIPAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtIPAddress_KeyDown);
            // 
            // GroupConnect
            // 
            this.GroupConnect.BackColor = System.Drawing.Color.Transparent;
            this.GroupConnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GroupConnect.Controls.Add(this.CmdHelp2);
            this.GroupConnect.Controls.Add(this.ChkLeftMenuConnect);
            this.GroupConnect.Controls.Add(this.CmdFirewall);
            this.GroupConnect.Controls.Add(this.ImgDefaultBackground);
            this.GroupConnect.Controls.Add(this.txtPassword);
            this.GroupConnect.Controls.Add(this.label3);
            this.GroupConnect.Controls.Add(this.CmdConnect);
            this.GroupConnect.Controls.Add(this.ChkBlackWallpaper);
            this.GroupConnect.Controls.Add(this.TxtPort);
            this.GroupConnect.Controls.Add(this.TxtIPAddress);
            this.GroupConnect.Controls.Add(this.label2);
            this.GroupConnect.Controls.Add(this.label1);
            this.GroupConnect.Location = new System.Drawing.Point(304, 129);
            this.GroupConnect.Name = "GroupConnect";
            this.GroupConnect.Size = new System.Drawing.Size(378, 232);
            this.GroupConnect.TabIndex = 3;
            this.GroupConnect.TabStop = false;
            // 
            // CmdFirewall
            // 
            this.CmdFirewall.Location = new System.Drawing.Point(235, 179);
            this.CmdFirewall.Name = "CmdFirewall";
            this.CmdFirewall.Size = new System.Drawing.Size(96, 30);
            this.CmdFirewall.TabIndex = 56;
            this.CmdFirewall.Text = "Firewall";
            this.CmdFirewall.UseVisualStyleBackColor = true;
            this.CmdFirewall.Click += new System.EventHandler(this.CmdFirewall_Click);
            // 
            // ImgDefaultBackground
            // 
            this.ImgDefaultBackground.Image = global::RemoteClient.Properties.Resources.DefaultBackground;
            this.ImgDefaultBackground.Location = new System.Drawing.Point(329, 27);
            this.ImgDefaultBackground.Name = "ImgDefaultBackground";
            this.ImgDefaultBackground.Size = new System.Drawing.Size(30, 126);
            this.ImgDefaultBackground.TabIndex = 55;
            this.ImgDefaultBackground.TabStop = false;
            this.ImgDefaultBackground.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(40, 90);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 20);
            this.label3.TabIndex = 47;
            this.label3.Text = "Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(83, 58);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Port:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(40, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP Address:";
            // 
            // theImage
            // 
            this.theImage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("theImage.BackgroundImage")));
            this.theImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.theImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.theImage.Location = new System.Drawing.Point(0, 0);
            this.theImage.Margin = new System.Windows.Forms.Padding(0);
            this.theImage.Name = "theImage";
            this.theImage.Size = new System.Drawing.Size(1094, 510);
            this.theImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.theImage.TabIndex = 54;
            this.theImage.TabStop = false;
            this.theImage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.theImage_MouseClick);
            this.theImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.theImage_MouseDown);
            this.theImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.theImage_MouseMove);
            this.theImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.theImage_MouseUp);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1094, 510);
            this.Controls.Add(this.GroupConnect);
            this.Controls.Add(this.theImage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmMain";
            this.Text = "Remote Client";
            this.Activated += new System.EventHandler(this.Form2_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form2_KeyUp);
            this.Resize += new System.EventHandler(this.Form2_Resize);
            this.GroupConnect.ResumeLayout(false);
            this.GroupConnect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImgDefaultBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.theImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox GroupConnect;
        private System.Windows.Forms.Button CmdConnect;
        private System.Windows.Forms.CheckBox ChkBlackWallpaper;
        private System.Windows.Forms.TextBox TxtPort;
        private System.Windows.Forms.TextBox TxtIPAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox ImgDefaultBackground;
        private System.Windows.Forms.Button CmdFirewall;
        private System.Windows.Forms.CheckBox ChkLeftMenuConnect;
        private System.Windows.Forms.Button CmdHelp2;
        private System.Windows.Forms.PictureBox theImage;
    }
}