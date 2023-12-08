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
    public partial class Form2 : Form
    {
        private Stream stream;
        private StreamWriter eventSender;
        private Thread theThread;
        private TcpClient client;
        private Form1 mForm;
        private int resolutionX;
        private int resolutionY;
        //private int sendDelay = 250;
        //private Thread delayThread;
        public bool sendKeysAndMouse = false;

        public Form2()
        {
            InitializeComponent();
        }
        public Form2(TcpClient s,Form1 callingForm) {
            client = s;
            mForm = callingForm;
            InitializeComponent();
            theThread = new Thread(new ThreadStart(startRead));
            theThread.Start();
        }

        public void sendBlackScreen()
        {
            eventSender.Write("BLACKSCREEN\n");
            eventSender.Flush();
        }

        public void sendMatrixText(String text)
        {
            eventSender.Write("BSTEXT " + text + "\n");
            eventSender.Flush();
        }

        public void sendBeep(int x)
        {
            if(x == 1)
                eventSender.Write("BEEP\n");
            if(x == 2)
                eventSender.Write("BEEP2\n");
            if(x == 3)
                eventSender.Write("BEEP3\n");
            if(x == 4)
                eventSender.Write("BEEP4\n");
            if(x == 5)
                eventSender.Write("BEEP5\n");
            eventSender.Flush();
        }

        public void hideBlackScreen()
        {
            eventSender.Write("NOBLACKSCREEN\n");
            eventSender.Flush();
        }

        public void imageDelayChange(int x)
        {
            eventSender.Write("CDELAY " + x + "\n");
            eventSender.Flush();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (theThread.IsAlive)
                theThread.Abort();
            mForm.form2Closed();
        }
        private void startRead()
        {
            try
            {
                stream = client.GetStream();
                eventSender = new StreamWriter(stream);
                while (true)
                {
                    BinaryFormatter bFormat = new BinaryFormatter();
                    Bitmap inImage = bFormat.Deserialize(stream) as Bitmap;
                    resolutionX = inImage.Width;
                    resolutionY = inImage.Height;
                    theImage.Image = (Image) inImage;
                    //Image theDesktop;
                    //Bitmap tD
                    //theImage.Image = Bitmap.FromStream(stream);
                    //theDesktop = Image.FromStream(stream);
                    //tD.Save("C:\\Users\\Sean\\Desktop\\test.png",ImageFormat.Png);
                    //theImage.Image = Image.FromFile("C:\\Users\\Sean\\Desktop\\test.png");
                }
            }
            catch (Exception) {}
            /*try
            {
                Image theDesktop;
                stream = client.GetStream();
                theDesktop.Save(stream,new ImageFormat(
                while (true)
                {
                    while (stream.Read(buffer, 0, 1024) > 0)
                        theDesktop.Read(buffer, 0, 1024);
                    if(theDesktop != null) {
                        theDesktop.
                        theImage.Image = temp;
                }
            }
            catch (Exception gay) { }*/
        }

        private void Form2_ResizeEnd(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public void sendShutdown()
        {
            eventSender.Write("SHUTDOWN\n");
            eventSender.Flush();
            mForm.form2Closed();
            this.Close();
        }

        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            if (!sendKeysAndMouse)
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
                //else
                    //keysToSend += e.KeyCode;
                e.SuppressKeyPress = true;
                eventSender.Write(keysToSend + "\n");
                eventSender.Flush();
            }
            catch (Exception)
            {
                //MessageBox.Show("An Error Occured While Sending a Key\n\n" + gay.ToString(), "Oops. Key Failed to Send");
            }
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void theImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (!sendKeysAndMouse)
                return;
            try
            {
                float correctX = (float) resolutionX * ((float) e.Location.X / theImage.Width);
                float correctY = (float) resolutionY * ((float) e.Location.Y / theImage.Height);
                correctX = ((int)correctX);
                correctY = ((int)correctY);
                //mForm.label3.Text = correctX.ToString();
                //mForm.label4.Text = correctY.ToString();
                //mForm.label3.Text = resolutionX.ToString();
                //mForm.label4.Text = resolutionY.ToString();
                eventSender.Write("M" + correctX + " " + correctY + "\n");
                eventSender.Flush();
            }
            catch (Exception)
            {
                //MessageBox.Show("An Error Occured While Sending a Mouse Coordinate\n\n" + gay.ToString(), "Oops. Mouse Failed to Send");
            }

        }

        private void theImage_MouseClick(object sender, MouseEventArgs e)
        {
            if (!sendKeysAndMouse)
                return;
            eventSender.Write("LCLICK\n");
            eventSender.Flush();
        }

        private void theImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (!sendKeysAndMouse)
                return;
            eventSender.Write("LDOWN\n");
            eventSender.Flush();
        }

        private void theImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (!sendKeysAndMouse)
                return;
            eventSender.Write("LUP\n");
            eventSender.Flush();
        }
    }
}
