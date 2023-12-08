namespace RemoteServer
{
    partial class FrmService
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmService));
            this.CmdClear = new System.Windows.Forms.Button();
            this.TxtDubug = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.CmdHide = new System.Windows.Forms.Button();
            this.TxtPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtPassWord = new System.Windows.Forms.TextBox();
            this.CmdFirewall = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CmdClear
            // 
            this.CmdClear.Location = new System.Drawing.Point(29, 150);
            this.CmdClear.Name = "CmdClear";
            this.CmdClear.Size = new System.Drawing.Size(75, 28);
            this.CmdClear.TabIndex = 0;
            this.CmdClear.Text = "Clear";
            this.CmdClear.UseVisualStyleBackColor = true;
            this.CmdClear.Click += new System.EventHandler(this.CmdClear_Click);
            this.CmdClear.MouseHover += new System.EventHandler(this.CmdClear_MouseHover);
            // 
            // TxtDubug
            // 
            this.TxtDubug.Location = new System.Drawing.Point(352, 5);
            this.TxtDubug.Multiline = true;
            this.TxtDubug.Name = "TxtDubug";
            this.TxtDubug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtDubug.Size = new System.Drawing.Size(430, 422);
            this.TxtDubug.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // CmdHide
            // 
            this.CmdHide.Location = new System.Drawing.Point(145, 149);
            this.CmdHide.Name = "CmdHide";
            this.CmdHide.Size = new System.Drawing.Size(75, 29);
            this.CmdHide.TabIndex = 2;
            this.CmdHide.Text = "Hide";
            this.CmdHide.UseVisualStyleBackColor = true;
            this.CmdHide.Click += new System.EventHandler(this.CmdHide_Click);
            // 
            // TxtPort
            // 
            this.TxtPort.Location = new System.Drawing.Point(208, 8);
            this.TxtPort.Name = "TxtPort";
            this.TxtPort.Size = new System.Drawing.Size(108, 22);
            this.TxtPort.TabIndex = 4;
            this.TxtPort.Text = "4000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "TCP Listen port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Password";
            // 
            // TxtPassWord
            // 
            this.TxtPassWord.Location = new System.Drawing.Point(208, 41);
            this.TxtPassWord.Name = "TxtPassWord";
            this.TxtPassWord.Size = new System.Drawing.Size(108, 22);
            this.TxtPassWord.TabIndex = 6;
            this.TxtPassWord.Text = "letmein";
            // 
            // CmdFirewall
            // 
            this.CmdFirewall.Location = new System.Drawing.Point(29, 94);
            this.CmdFirewall.Name = "CmdFirewall";
            this.CmdFirewall.Size = new System.Drawing.Size(123, 30);
            this.CmdFirewall.TabIndex = 14;
            this.CmdFirewall.Text = "Add firewall rule";
            this.CmdFirewall.UseVisualStyleBackColor = true;
            this.CmdFirewall.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CmdFirewall_MouseClick);
            // 
            // FrmService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 431);
            this.Controls.Add(this.TxtDubug);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtPassWord);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TxtPort);
            this.Controls.Add(this.CmdHide);
            this.Controls.Add(this.CmdClear);
            this.Controls.Add(this.CmdFirewall);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmService";
            this.Text = "Remote Desktop Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CmdClear;
        private System.Windows.Forms.TextBox TxtDubug;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button CmdHide;
        private System.Windows.Forms.TextBox TxtPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtPassWord;
        private System.Windows.Forms.Button CmdFirewall;
    }
}

