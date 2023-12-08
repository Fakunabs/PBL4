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


namespace RemoteClient
{
    public partial class FrmMain : Form
    {
        private bool Closing = false;
        private Stream stream;
        private StreamWriter eventSender;
        private Thread theThread;
        private TcpClient Tcpclient;
        private int resolutionX;
        private int resolutionY;
        private bool FirstConnect = true;


        public FrmMain()
        {
            InitializeComponent();
        }


        public void sendBlackScreen()
        {
            SafeSendValue("BLACKSCREEN");
        }

        public void sendMatrixText(String text)
        {
            SafeSendValue("BSTEXT " + text);
        }

        public void SendScreenSize()
        {
            SafeSendValue("SCREEN " + Settings.ScreenServerX + " " + Settings.ScreenServerY);
        }

        public void SendCommand(string Cmd)
        {
            SafeSendValue("CMD " + Cmd.ToUpper());
        }

        public bool SafeSendValue(string Text)
        {
            int Shift = 45;
            if (Text.Length == 1) Shift = 77;//Change the shift for a single key press just to confuse anyone looking
            if (!Settings.Connected) return false;
            try
            {
                string T = Helper.XorString(Text, Shift, true);
                eventSender.Write(T + "\n");
                eventSender.Flush();
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        public Bitmap ResizeImage(Bitmap B,  int Width,int Height)
        {//Make the image fit our screen if it was compressed down to speed the network up
            Bitmap BNew = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(BNew);
            float scaleFactorX  = (float)Width / (float)B.Width;
            float scaleFactorY = (float)Height / (float)B.Height;
            G.ScaleTransform(scaleFactorX/1.25f, scaleFactorY/1.25f);
            G.DrawImage(B, 0, 0);
            return BNew;
        }

        private string LastClipboardText = "";
        public void SendClipboard()
        {//Only sends text but would be nice to send/recive files and images too 
            string Text = ClipboardAsync.GetText();
            if (Text != null && Text.Length > 0 && Text != LastClipboardText)
            {
                SafeSendValue("CLIPBOARD " + Text.Replace(Environment.NewLine,"#NL#").Replace("\n","#NL#").Replace("'","#SQ#"));
                LastClipboardText = Text;
            }
        }

        public MemoryStream Decrypt(MemoryStream MSin)
        {//We just add PNG back in to the begining of the image
            byte[] Dummey = UTF8Encoding.UTF8.GetBytes("_PNG\r\n");
            MSin.Position = 0;
            Dummey[0] = 137;
            MSin.Write(Dummey, 0, 6);
            return MSin;
        }

        private void ReadInfo(string Info)
        {
            Info = Info.Substring(6);
            if (Info.StartsWith("SCREEN_"))
            {//Looks like the screen res has been changed on the server
                string[] Data = Info.Split('_');
                Settings.ScreenClientX = int.Parse(Data[1]);
                Settings.ScreenClientY = int.Parse(Data[2]);
                Settings.ScreenServerX = Settings.ScreenClientX;
                Settings.ScreenServerY = Settings.ScreenClientY;
                //Settings.LoadProfile();//We might have a profile for this screen on the server machine already;
                SendScreenSize();
            }
        }

        private Bitmap BitmapFromStream(bool Encrypted)
        {//Here we wait for a new screen-shot or clip-board text to come in from the server
            Bitmap Image = null;
            BinaryFormatter bFormat = new BinaryFormatter();
            MemoryStream MSin = null;
            try
            {
              
                if (Encrypted)
                {
                    MSin = bFormat.Deserialize(stream) as MemoryStream;
                    if (MSin == null || MSin.Length <200)
                    {
                        if (MSin != null)
                        {
                            string Data = UTF8Encoding.UTF8.GetString(MSin.ToArray());
                            string ClipboardText =Helper.XorString( Data.Replace("#NL#", Environment.NewLine),34,false);
                            if (ClipboardText.StartsWith("#CLIPBOARD#"))
                            {//Yes the server sent the clip-board text
                                LastClipboardText = ClipboardText.Substring(11);
                                ClipboardAsync.SetText(LastClipboardText);
                            }
                            else if (ClipboardText.StartsWith("#INFO#"))
                                ReadInfo(ClipboardText);//Looks like the screen res on the server has been changed 
                            return new Bitmap(1, 1);
                        }
                    }
                    MemoryStream MSout = Decrypt(MSin);//Well OK it's not full on encryption
                    Image = new Bitmap(MSout);
                    Settings.Encrypted = true;
                }
                else
                {//Not encrypted so just read the image and if its clip-board text then error trap later will catch it
                    Image = bFormat.Deserialize(stream) as Bitmap;
                    Settings.Encrypted = false;
                }
                return Image;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        private void startRead()
        {//Runs on its own thread and keeps looping to read anything that the server sends us
            try
            {
                Thread.Sleep(500);
                while (true)
                {
                    TimeSpan TS = DateTime.Now - DateMTitle;
                    if (TS.TotalSeconds > 15) { this.Invoke((MethodInvoker)delegate() { theImage.Focus(); }); }
                    if (Settings.SendKeysAndMouse) SendClipboard();
                    try
                    {
                        Bitmap inImage = BitmapFromStream(Settings.Encrypted);
                        if (inImage ==null) inImage=BitmapFromStream(!Settings.Encrypted);
                        if (Settings.Scale) inImage = ResizeImage(inImage, 1920, 1080);
                        resolutionX = inImage.Width;
                        resolutionY = inImage.Height;
                        if (resolutionX>5) theImage.Image = (Image)inImage;
                        if (Settings.SendKeysAndMouse && resolutionX> 5)
                        {
                            Color C1 = inImage.GetPixel(0, 0);//Our hidden pixel used to send the mouse cursor type enbeded in the image
                            Color C2 = inImage.GetPixel(0, 1);//Our hidden pixed used to know if the server is in metrol-mode
                            Settings.IsMetro = (C2.R == 0 && C2.G == 0 && C2.B == 0);
                            this.Invoke((MethodInvoker)delegate() { this.Cursor = WinCursor.ColorToCursor(C1); });
                        }
                    }
                    catch (Exception Ex)
                    {//This data would not convert to a image so empty the stream if we can 
                        if (Closing) return;
                        string Data = "";
                        int Count = 0;
                        if (!Settings.Connected) return ;
                        if (!SafeSendValue("LOCK"))//Stop the server sending more data
                        {//Looks like the server has crashed
                            this.Invoke((MethodInvoker)delegate()
                            {
                                CmdDisconect_MouseClick(null, null);
                            });
                            return;
                        }
                        while (Tcpclient.Available > 0 && Count<10)
                        {
                            Count++;
                            byte[] Buffer = new byte[Tcpclient.Available];
                            stream.Read(Buffer, 0, Buffer.Length);
                            Data += UTF8Encoding.UTF8.GetString(Buffer);
                        }
                        Data = Data.Trim();
                        SafeSendValue("UNLOCK");//Let the server send more data
                        if (Settings.SendKeysAndMouse && Data.Length < 100000 && Data.Length >0)
                        {//Looks like clipboard or screen info data so set the local clipboard or set screen sizes
                            string ClipboardText =Helper.XorString( Data,34,false).Replace("#NL#", Environment.NewLine).Replace("#SQ#","'");
                            if (ClipboardText.StartsWith("#CLIPBOARD#"))
                            {
                                LastClipboardText = ClipboardText.Substring(11);
                                ClipboardAsync.SetText(LastClipboardText);
                            }
                            else if (ClipboardText.StartsWith("#INFO#"))
                                ReadInfo(ClipboardText);

                        }
                    }
                }
            }
            catch (Exception Ex) 
            {
               //if (!Closing)
               // MessageBox.Show("We crashed " + Ex.Message);
            }
        }


        private void Form2_Load(object sender, EventArgs e)
        {
#if !DEBUG
            FireWall.ProgramName = Settings.MainProgramName;
#endif
            this.Text = Settings.MainProgramName;
            //Settings.LoadSettings();
            this.TxtIPAddress.Text  = Settings.IP;
            this.txtPassword.Text = Settings.PassWord;
            TxtPort.Text = Settings.Port.ToString();
            Form2_Resize(null, null);
        }

        public void sendShutdown()
        {
            SafeSendValue("SHUTDOWN");
            this.Close();
        }

        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Settings.SendKeysAndMouse || !Settings.Connected)
                return;
            try
            {
                String keysToSend = "";
                if (e.Shift)
                    keysToSend += "+";
                if (e.Alt)
                    keysToSend += "%";
                if (e.Control)
                    keysToSend += "^";

                if (e.KeyValue >= 65 && e.KeyValue <= 90)
                    keysToSend += e.KeyCode.ToString().ToLower();
                else if (e.KeyCode.ToString().Equals("Back"))
                    keysToSend += "{BS}";
                else if (e.KeyCode.ToString().Equals("Pause"))
                    keysToSend += "{BREAK}";
                else if (e.KeyCode.ToString().Equals("Capital"))
                    keysToSend += "{CAPSLOCK}";
                else if (e.KeyValue == 144)
                    keysToSend += "{NUMLOCK}";
                else if (e.KeyCode.ToString().Equals("Space"))
                    keysToSend += " ";
                else if (e.KeyCode.ToString().Equals("Home"))
                    keysToSend += "{HOME}";
                else if (e.KeyCode.ToString().Equals("Return"))
                    keysToSend += "{ENTER}";
                else if (e.KeyCode.ToString().Equals("End"))
                    keysToSend += "{END}";
                else if (e.KeyCode.ToString().Equals("Tab"))
                    keysToSend += "{TAB}";
                else if (e.KeyCode.ToString().Equals("Escape"))
                    keysToSend += "{ESC}";
                else if (e.KeyCode.ToString().Equals("Insert"))
                    keysToSend += "{INS}";
                else if (e.KeyCode.ToString().Equals("Up"))
                    keysToSend += "{UP}";
                else if (e.KeyCode.ToString().Equals("Down"))
                    keysToSend += "{DOWN}";
                else if (e.KeyCode.ToString().Equals("Left"))
                    keysToSend += "{LEFT}";
                else if (e.KeyCode.ToString().Equals("Right"))
                    keysToSend += "{RIGHT}";
                else if (e.KeyCode.ToString().Equals("PageUp"))
                    keysToSend += "{PGUP}";
                else if (e.KeyCode.ToString().Equals("Next"))
                    keysToSend += "{PGDN}";
                else if (e.KeyCode.ToString().Equals("Tab"))
                    keysToSend += "{TAB}";
                else if (e.KeyCode.ToString().Equals("D1"))
                    keysToSend += "1";
                else if (e.KeyCode.ToString().Equals("D2"))
                    keysToSend += "2";
                else if (e.KeyCode.ToString().Equals("D3"))
                    keysToSend += "3";
                else if (e.KeyCode.ToString().Equals("D4"))
                    keysToSend += "4";
                else if (e.KeyCode.ToString().Equals("D5"))
                    keysToSend += "5";
                else if (e.KeyCode.ToString().Equals("D6"))
                    keysToSend += "6";
                else if (e.KeyCode.ToString().Equals("D7"))
                    keysToSend += "7";
                else if (e.KeyCode.ToString().Equals("D8"))
                    keysToSend += "8";
                else if (e.KeyCode.ToString().Equals("D9"))
                    keysToSend += "9";
                else if (e.KeyCode.ToString().Equals("D0"))
                    keysToSend += "0";
                else if (e.KeyCode.ToString().Equals("F1"))
                    keysToSend += "{F1}";
                else if (e.KeyCode.ToString().Equals("F2"))
                    keysToSend += "{F2}";
                else if (e.KeyCode.ToString().Equals("F3"))
                    keysToSend += "{F3}";
                else if (e.KeyCode.ToString().Equals("F4"))
                    keysToSend += "{F4}";
                else if (e.KeyCode.ToString().Equals("F5"))
                    keysToSend += "{F5}";
                else if (e.KeyCode.ToString().Equals("F6"))
                    keysToSend += "{F6}";
                else if (e.KeyCode.ToString().Equals("F7"))
                    keysToSend += "{F7}";
                else if (e.KeyCode.ToString().Equals("F8"))
                    keysToSend += "{F8}";
                else if (e.KeyCode.ToString().Equals("F9"))
                    keysToSend += "{F9}";
                else if (e.KeyCode.ToString().Equals("F10"))
                    keysToSend += "{F10}";
                else if (e.KeyCode.ToString().Equals("F11"))
                    keysToSend += "{F11}";
                else if (e.KeyCode.ToString().Equals("F12"))
                    keysToSend += "{F12}";
                else if (e.KeyValue == 186)
                    keysToSend += "{;}";
                else if (e.KeyValue == 222)
                    keysToSend += "'";
                else if (e.KeyValue == 191)
                    keysToSend += "/";
                else if (e.KeyValue == 190)
                    keysToSend += ".";
                else if (e.KeyValue == 188)
                    keysToSend += ",";
                else if (e.KeyValue == 219)
                    keysToSend += "{[}";
                else if (e.KeyValue == 221)
                    keysToSend += "{]}";
                else if (e.KeyValue == 220)
                    keysToSend += "\\";
                else if (e.KeyValue == 187)
                    keysToSend += "{=}";
                else if (e.KeyValue == 189)
                    keysToSend += "{-}";
                else if (e.KeyCode.ToString().ToLower().StartsWith("numpad"))
                    keysToSend += e.KeyCode.ToString().Substring(6);
                else if (e.KeyCode ==Keys.Divide)
                    keysToSend += "/";
                else if (e.KeyCode==Keys.Decimal)
                    keysToSend += ".";
                else if (e.KeyCode==Keys.Subtract)
                    keysToSend += "-";
                else if (e.KeyCode==Keys.Add)
                    keysToSend += "+=";
                else if (e.KeyCode==Keys.Multiply)
                    keysToSend += "*";
                else if (e.KeyCode == Keys.Delete)
                    keysToSend += "+{DEL}";
                else
                {
                    e.SuppressKeyPress = true;
                    return;
                }
                e.SuppressKeyPress = true;
                SafeSendValue(keysToSend);
            }
            catch (Exception)
            {

            }
        }


        private void theImage_MouseMove(object sender, MouseEventArgs e)
        {//Keep sending our mouse X,Y to the server and also a nudge in metro mode for pen devices to try and scroll the screen
            if (!Settings.SendKeysAndMouse)
                return;
            if (this.WindowState == FormWindowState.Maximized)
            {
                if (e.X < 9 && e.Y < 9 && Settings.IsMetro)
                {
                    SendCommand("SLEFT"); Cursor.Position = new Point(1, 1);
                    SafeSendValue("M0 0" );
                    return; 
                }
                if (e.X < 9 && e.Y >30 && Settings.IsMetro) { SendCommand("SLEFT"); Cursor.Position = new Point(e.X + 50, e.Y); return; }
                if (e.Y < 9 && e.X >50 && Settings.IsMetro) { SendCommand("SUP"); Cursor.Position = new Point(e.X, e.Y + 50); return; }
                if (e.X > Screen.PrimaryScreen.Bounds.Width && e.Y >30 && Settings.IsMetro) { SendCommand("SRIGHT"); Cursor.Position = new Point(e.X - 50, e.Y); return; }
                if (e.Y > Screen.PrimaryScreen.Bounds.Height - 30 && Settings.IsMetro) {SendCommand("SDOWN"); Cursor.Position = new Point(e.X,e.Y-50); return; }
                if (e.Location.Y < 11 && e.Location.X > Screen.PrimaryScreen.Bounds.Width - 11)
                {
                    SendCommand("METRO");
                    Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Width - 300, 300);
                    Thread.Sleep(200);
                    Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Width - 310, 310);
                    Thread.Sleep(200);
                    Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Width - 30, Screen.PrimaryScreen.Bounds.Height/2);
                    return;
                }
            }
            try
            {//We are not full screen so scale the mouse X,Y to suite the current size 
                float correctX = (float)Settings.ScreenClientX * ((float)e.Location.X / (theImage.Width - theImage.Padding.Left - theImage.Padding.Right  ));
                float correctY = (float)Settings.ScreenClientY * ((float)e.Location.Y / (theImage.Height-theImage.Padding.Top ));
                correctX = ((int)correctX);
                correctY = ((int)correctY);
                SafeSendValue("M" + correctX + " " + correctY);
            }
            catch (Exception)
            {
               
            }

        }

        private void theImage_MouseClick(object sender, MouseEventArgs e)
        {
            if (!Settings.SendKeysAndMouse)
                return;
            if (e.Button ==  System.Windows.Forms.MouseButtons.Left)
                SafeSendValue("LCLICK");
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                SafeSendValue("RCLICK");
        }

        private void theImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (Settings.Connected)
                this.Invoke((MethodInvoker)delegate() { });
            if (!Settings.SendKeysAndMouse)
                return;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                SafeSendValue("LDOWN");
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                SafeSendValue("RDOWN");
        }

        private void theImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (!Settings.SendKeysAndMouse)
                return;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                SafeSendValue("LUP");
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                SafeSendValue("RUP");
        }


        private void RunConnect()
        {//Connect to the server, send our password and get ready to rock and roll
            try
            {

            if (Helper.IsIP4Address(TxtIPAddress.Text.Trim()))
                this.Tcpclient = new TcpClient(TxtIPAddress.Text.ToString(), int.Parse(TxtPort.Text.ToString()));
            else
            {
                string IP = Helper.GetIP(TxtIPAddress.Text.Trim());
                if (IP.Length == 0)
                {
                    MessageBox.Show("Sorry we cannot resolve the name '" + TxtIPAddress.Text + "' to an IP-Address", "Remote desktop", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Tcpclient = new TcpClient(TxtIPAddress.Text.ToString(), int.Parse(TxtPort.Text.ToString()));
            }
            Stream S = Tcpclient.GetStream();
            StreamWriter SW = new StreamWriter(S);
            //this.LblTitle.Text  = TxtIPAddress.Text + ":" + TxtPort.Text;
            this.Text = TxtIPAddress.Text + ":" + TxtPort.Text;
            SW.Write(Helper.XorString("CMD PASSWORD " + txtPassword.Text.Trim(),45,true) + "\n");
            SW.Flush();
            Settings.IP = TxtIPAddress.Text.Trim();
            Settings.Port = int.Parse(TxtPort.Text);
            Settings.PassWord = txtPassword.Text.Trim();
            //Settings.SaveLogonDetails();
            GroupConnect.Visible = false;
            byte[] Buffer = new byte[1024];
            int Size = S.Read(Buffer, 0, Buffer.Length);
            string Msg = ASCIIEncoding.ASCII.GetString(Buffer, 0, Size).Replace("X=", "").Replace("Y=", "");
            bool FirstConnect = bool.Parse(Msg.Split(' ')[4]);
            Settings.ScreenServerX = int.Parse(Msg.Split(' ')[0]);
            Settings.ScreenServerY = int.Parse(Msg.Split(' ')[1]);
            Settings.ScreenClientX = int.Parse(Msg.Split(' ')[2]);
            Settings.ScreenClientY = int.Parse(Msg.Split(' ')[3]);
            //Settings.LoadProfile();
            SW.Write(Helper.XorString("SCREEN " + Settings.ScreenServerX + " " + Settings.ScreenServerY,45,true) + "\n");
            SW.Flush();
            //this.TrackWidth.Value = TrackWidth.Maximum + TrackWidth.Minimum - Settings.ScreenServerX;
            //this.TrackHeight.Value = TrackHeight.Maximum + TrackHeight.Minimum - Settings.ScreenServerY;           
            string Text = ClipboardAsync.GetText();
            LastClipboardText = Text;
            stream = Tcpclient.GetStream();
            Settings.Connected = true;
            GroupConnect.Visible = false;
            GroupConnect.Enabled  = false;
            eventSender = new StreamWriter(stream);
            this.WindowState = FormWindowState.Maximized;
            theThread = new Thread(new ThreadStart(startRead));
            theThread.Start();
            SendDefaults(FirstConnect);
            theImage.Focus();
            }
            catch (Exception problem)
            {
                MessageBox.Show("Invalid IPAddress, Invalid Port, Failed Internet Connection, or Cannot connect to client for some reason, check firewall in windows and router.\n\nTechnical Data:\n**************************************************\n" + problem.ToString(), "You're a Failure!");
                GroupConnect.Visible = true;
            }
        }


        private void SendDefaults(bool FirstConnect)
        {
            if (FirstConnect)
                SendScreenSize();
            TrackSpeed_Scroll(null, null);
            Thread TH = new Thread(new ThreadStart(SendDefaultsNow));
            TH.Start();
        }

        private void SendKeySync()
        {//Deals with cap-lock, num-lock and keeps the server/client in sync with these keys
            if (Settings.SendKeysAndMouse && Settings.Connected)
                SendCommand("KEYSYNC " + Control.IsKeyLocked(Keys.CapsLock).ToString() + " " + Control.IsKeyLocked(Keys.NumLock).ToString() + " " + Control.IsKeyLocked(Keys.Scroll).ToString()); Thread.Sleep(20);
        }

        private void SendDefaultsNow()
        {
            SendKeySync(); Thread.Sleep(20);
            SendCommand("WALLPAPER " + ChkBlackWallpaper.Checked.ToString()); Thread.Sleep(20);
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {//Stop the server from keep sending us the desktop, chill out and have a sleep because we are
                if (Settings.Connected) { Settings.Sleep = true; SendCommand("SLEEP TRUE"); }
                return;
            }

            if (Settings.Connected && Settings.Sleep ) { Settings.Sleep = false; SendCommand("SLEEP FALSE"); }
            if (this.WindowState == FormWindowState.Maximized)
            {
                int Padding = 0;
                if (Settings.ScreenServerX >= 1800) Padding = 3;//Little tweak to make the screen fit a bit better
                else if (Settings.ScreenServerX > 1500) Padding = 2;
                else if (Settings.ScreenServerX > 1200) Padding = 1;
                if (Settings.Connected && Settings.Padding != Padding)
                {
                    Settings.Padding = Padding;
                    SendCommand("PADDING " + Padding);
                }
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.theImage.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            }
            else
            {
                this.theImage.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
            }
        }

      
        private DateTime DateMTitle = DateTime.Now.AddDays(-1);//Wot should this not be at the top or do you think it might work just as well here 
        private void TaskBar_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            DateMTitle = DateTime.Now;
        }


        private void ChkDebug_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void ChkScale_CheckedChanged(object sender, EventArgs e)
        {        }

        private void ChkResoloution_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void ChkEncrypted_CheckedChanged(object sender, EventArgs e)
        {        }

        private void ChkSendKeyboardAndMouse_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void ChkBlackWallpaper_CheckedChanged(object sender, EventArgs e)
        {
            Settings.BlackWallpaper = ChkBlackWallpaper.Checked;
        }

        private void CmdConnect_Click(object sender, EventArgs e)
        {//Use mouse down incase the button gets the focus
        }

        private void TrackSpeed_Scroll(object sender, EventArgs e)
        {        }

        private void CmdRestore_Click(object sender, EventArgs e)
        {
        }

        private void CmdMinimize_Click(object sender, EventArgs e)
        {
        }

        private void CmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CmdClose_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            DateMTitle = DateTime.Now;
        }

        private void CmdRestore_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            DateMTitle = DateTime.Now;
        }

        private void CmdMinimize_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            DateMTitle = DateTime.Now;
        }

        private void ChkBlackWallpaper_CheckedChanged_1(object sender, EventArgs e)
        {
            Settings.BlackWallpaper = ChkBlackWallpaper.Checked;
        }

        private void GroupConected_MouseHover(object sender, EventArgs e)
        {
            DateMTitle = DateTime.Now;
        }

        private void TxtIPAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter )
            {
                e.SuppressKeyPress = true;
                CmdConnect.Focus();
                CmdConnect_MouseClick(null, null);
            }
            
        }

        private void TxtPort_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                CmdConnect.Focus();
                CmdConnect_MouseClick(null, null);
            }
        }

        private void CmdMenuSmall_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            DateMTitle = DateTime.Now;
        }

        private void CmdSendStart_Click(object sender, EventArgs e)
        {
            SendCommand("SHOWSTART");
            theImage.Focus();
        }

        private void CmdSendCtrlAltDel_Click(object sender, EventArgs e)
        {
            SendCommand("CTRLALTDELETE");
            theImage.Focus();
        }

        private void CmdMetro_Click(object sender, EventArgs e)
        {
            SendKeySync();
            SendCommand("METRO");
            Thread.Sleep(500);
            if (this.WindowState == FormWindowState.Maximized) Cursor.Position = new Point(Screen.PrimaryScreen.Bounds.Width - 30, Screen.PrimaryScreen.Bounds.Height / 2);
            theImage.Focus();
        }

        private void CmdMenu_MouseClick(object sender, MouseEventArgs e)
        {
            if (!Settings.Connected) return;
            this.Cursor = Cursors.Default;
        }

        private void CmdMenuSmall_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void CmdShutDown_Click(object sender, EventArgs e)
        {
            DialogResult reallyShutdown = MessageBox.Show("!! WARNING !!\n\nShutting down the server will leave you unable to reconnect until " +
                         "the computer restarts (IF The server is set to run on Startup). You will be unable " +
                         "to reconnect until this occurs.\n\nARE YOU SURE YOU WANT TO SHUTDOWN THE SERVER?", "!! WARNING !!", MessageBoxButtons.YesNo);
            if (reallyShutdown == DialogResult.Yes)
            {
                sendShutdown();
                Settings.Connected = false;
                CmdDisconect_MouseClick(null, null);

            }
        }

        private void CmdConnect_MouseClick(object sender, MouseEventArgs e)
        {
            if (FirstConnect)
            {
                FirstConnect = false;
                //try { Helper.AddDesktopShortcut();} catch { ;}
            }
            RunConnect();
        }

        private void CmdMenu_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            DateMTitle = DateTime.Now;
        }

        private void CmdFirewall_Click(object sender, EventArgs e)
        {
            if (Helper.IsUserAdministrator())
            {
                if (FireWall.AllowThisProgram("TCP", Settings.Port.ToString(), "", "OUT"))
                {
                    MessageBox.Show("New firewall rule added for TCP port " + Settings.Port, Settings.MainProgramName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            MessageBox.Show("Sorry but you need to run this program as an administrator to add new firewall rules", Settings.MainProgramName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

       

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                CmdConnect.Focus();
                CmdConnect_MouseClick(null, null);
            }
        }

        private void Form2_Activated(object sender, EventArgs e)
        {
            if (Settings.Connected && Settings.SendKeysAndMouse)
            {
             Thread TH=new Thread(SendKeySync);
             TH.Start();

            }
        }

        private void ChkLeftMenu_CheckedChanged(object sender, EventArgs e)
        {
            ChkLeftMenuConnect.Checked = Settings.LeftMenu;
            Form2_Resize(null, null);
        }

        private void ChkLeftMenuConnect_CheckedChanged(object sender, EventArgs e)
        {
            Settings.LeftMenu = ChkLeftMenuConnect.Checked;
        }

        private void CmdHelp_MouseClick(object sender, MouseEventArgs e)
        {

        }
        private void CmdDisconect_MouseClick(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
            //Settings.SaveSettings();
            this.Text = "Remote Client";
            txtPassword.Text = Settings.PassWord;
            TxtPort.Text = Settings.Port.ToString();
            TxtIPAddress.Text = Settings.IP;
            SendCommand("CLOSE");
            Settings.Connected = false;
            this.Tcpclient.Close();
            if (theThread != null && theThread.IsAlive)
                theThread.Abort();
            GroupConnect.Enabled = true;
            GroupConnect.Visible = true;
            theImage.Image = ImgDefaultBackground.Image;
            theThread = null;
            Form2_Resize(null, null);
            CmdConnect.Focus();
        }

        private void TrackHeight_Scroll(object sender, ScrollEventArgs e)
        {
            DateMTitle = DateTime.Now;
            SendScreenSize();
           // Settings.SaveProfile();
        }

        private void TrackWidth_Scroll(object sender, ScrollEventArgs e)
        {
            DateMTitle = DateTime.Now;
            SendScreenSize();
           // Settings.SaveProfile();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Closing = true;
            if (Settings.Connected) CmdDisconect_MouseClick(null, null);
        }          
    }
}
