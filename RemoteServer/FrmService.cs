using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Threading;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Media;
using System.Diagnostics;

namespace RemoteServer
{
    public partial class FrmService : Form
    {
        public FrmService()
        {
            InitializeComponent();

        }

       

        
        public void printDebug(string Msg,bool Force)
        {//This will be called on this forms thread
            if (Settings.Debug || Force)
            {
                TxtDubug.Text = Msg + Environment.NewLine + TxtDubug.Text;
                if (TxtDubug.Text.Length > 20000) TxtDubug.Text = TxtDubug.Text.Substring(10000);
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Settings.MainProgramName;
            bool B=Control.IsKeyLocked(Keys.CapsLock);
            this.WindowState = FormWindowState.Normal;
           // Server.ImgWinbackground = ImgWinbackground.Image;
            Settings.FormService = this;
            if (!Helper.IsUserAdministrator())
                printDebug("This program should be started with administrator rights to allow control of windows system forms and settings." + Environment.NewLine , true);
            if (Settings.FirstTime)
            {
                TxtPassWord.Text = Settings.PassWord;
                TxtPort.Text = Settings.Port.ToString();
            }
            else
                Server.ListenStart();
            this.Hide();
        }

       

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Server.ListenStop(true);
        }

        private void CmdClear_Click(object sender, EventArgs e)
        {
            TxtDubug.Text = "";
        }

        private void CmdClear_MouseHover(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.SizeAll;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           this.TxtDubug.Text = Cursor.Current.ToString() + " " + System.Windows.Forms.Cursor.Position.X;
          
        }

        private void CmdHide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void CmdFirewall_MouseClick(object sender, MouseEventArgs e)
        {
            if (Helper.IsUserAdministrator())
            {
                FireWall.ProgramName = Settings.MainProgramName;
                if (FireWall.AllowThisProgram("TCP", Settings.Port.ToString(), "", "IN"))
                {
                    MessageBox.Show("New firewall rule added for TCP port " + Settings.Port, Settings.MainProgramName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Server.ListenStart(); return;
                }
            }
            MessageBox.Show("Sorry but you need to run this program as an administrator to add new firewall rules", Settings.MainProgramName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Server.ListenStart();
        }
    }     
}