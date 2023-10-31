using MetroFramework.Forms;
using MSTSCLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EYERemoteDeskptopApp
{
    public partial class Form1 : MetroForm
    {
        bool pingable = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
               btnDisconnect.Enabled = false;
                GetMyIp();
                GetWindowName();

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error:"+ ex.Message, "some Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void GetWindowName()
        {
            //string MachineName1 = Environment.MachineName;


        }

        private void GetMyIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if(ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    txtYourIp.Text = ip.ToString();
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtClientIp.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("IP is Required", "Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtClientIp.Focus();
                    return;
                }
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;

                rdp.Server = txtClientIp.Text;
                // rdp.UserName = txtUserName.Text;
                rdp.UserName = "Administrator";


                IMsTscNonScriptable Secured = (IMsTscNonScriptable)rdp.GetOcx();
                Secured.ClearTextPassword = txtPassword.Text;
                rdp.Connect();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

                }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (rdp.Connected.ToString() == "1")
                rdp.Disconnect();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void txtClientIp_Leave(object sender, EventArgs e)
        {
            string MachineName = string.Empty;

            try
            {
                string ip = txtClientIp.Text;
                // MessageBox.Show(ip);
                System.Net.IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(ip.Trim());
                MachineName = hostEntry.HostName;

               lblClientsystemName.Text = "Client System Name"  + " " + MachineName;


            }
            catch (PingException)
            {

                throw;
            }
        }

        private void btnping_Click(object sender, EventArgs e)
        {
            try
            {
                Ping pinger = new Ping();
                string IpAddress = txtClientIp.Text;
                PingReply pingReply = pinger.Send(IpAddress);

                pingable = pingReply.Status == IPStatus.Success;

                if(pingable == true)
                {
                    MessageBox.Show("Connection Success","Status", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Connection Timed Out", "Status", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }



            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
