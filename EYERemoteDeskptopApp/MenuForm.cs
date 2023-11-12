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
    public partial class MenuForm : MetroForm
    {
      

        public MenuForm()
        {
            InitializeComponent();

        }

    

        private void MenuForm_Load(object sender, EventArgs e)
        {

        }

        private void btnViewSCR_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            
        }

        private void btnChatroom_Click(object sender, EventArgs e)
        {
            ChatRoomForm chat = new ChatRoomForm();
            chat.Show();
        }
    }
}
