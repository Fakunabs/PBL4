using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EYERemoteDeskptopApp
{
    public partial class Form1 : Form
    {
        private readonly TcpClient client = new TcpClient();
        private NetworkStream mainStream;
        private int portNumber;

        private static Image GrabDesktop()
        {
            Rectangle bound = Screen.PrimaryScreen.Bounds;
            Bitmap screenshot = new Bitmap(bound.Width, bound.Height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(screenshot);
            graphics.CopyFromScreen(bound.X, bound.Y, 0, 0, bound.Size, CopyPixelOperation.SourceCopy);


            return screenshot;
        }

        private void SendDesktopImage()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            mainStream = client.GetStream();
            binaryFormatter.Serialize(mainStream, GrabDesktop());
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnShareScreen.Enabled = false;
            GetMyIp();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SendDesktopImage();
        }
        private void GetMyIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    txtYourIP.Text = ip.ToString();

                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            portNumber = int.Parse(txtPort.Text);
            try
            {
                client.Connect(txtIP.Text, portNumber);
                btnConnect.Text = "Connected";
                MessageBox.Show("Connected");
                btnConnect.Enabled = false;
                btnShareScreen.Enabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to connect");
                btnConnect.Text = "Not connected";
            }
        }

        private void btnShareScreen_Click(object sender, EventArgs e)
        {
            if (btnShareScreen.Text.StartsWith("Share"))
            {
                timer1.Start();
                btnShareScreen.Text = "Stop Sharing";
            }
            else
            {
                timer1.Stop();
                btnShareScreen.Text = "Share Screen";
            }
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            new ScreenForm(int.Parse(txtListen.Text)).Show();
            btnListen.Enabled = false;
        }
    }
}
