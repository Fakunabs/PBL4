using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Shared;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleServer;

namespace EYERemoteDeskptopApp
{
    public partial class ChatRoomForm : Form
    {
        private room room1;
        private room room2;
        private room room3;
        private room room4;
        private room room5;

        private room selectedRoom = null;
        public ChatRoomForm()
        {
            InitializeComponent();
            room1 = new room("room1", "127.0.0.1", 1111);
            room2 = new room("room2", "127.0.0.1", 2222);
            room3 = new room("room3", "127.0.0.1", 3333);
            room4 = new room("room4", "127.0.0.1", 4444);
            room5 = new room("room5", "127.0.0.1", 5555);

        }
        public void AppendText(string str)
        {
            richTextBox1.Text += str + "\n";
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
        void connectToRoom(room rm, Boolean disconnectifConnected)
        {
            if (!rm.Connected)
            {
                Boolean connected = rm.ConnectionRoom.Connect(this, rm.Address, rm.Port, nickNameTextBox.Text);
                if (connected)
                {
                    connectButton.Text = "Disconnect";
                    rm.Connected = true;
                }
            }
            else if (disconnectifConnected)
            {
                rm.ConnectionRoom.Disconnect();
                rm.Connected = false;
                connectButton.Text = "Connect";
            }

            refreshList();

        }
        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            connectToRoom(selectedRoom, true);
        }

        private void connectToAllButton_Click(object sender, EventArgs e)
        {
            connectToRoom(room1, false);
            connectToRoom(room2, false);
            connectToRoom(room3, false);
            connectToRoom(room4, false);
            connectToRoom(room5, false);
        }

        private void sendTextButton_Click(object sender, EventArgs e)
        {
            if (!selectedRoom.Connected)
                return;

            selectedRoom.ConnectionRoom.SendText(textBox1.Text);
            textBox1.Text = "";
        }
        void refreshList()
        {
            listBox1.Items.Clear();

            string[] myRooms = new string[5];

            myRooms[0] = room1.Name + ConnectedOrNot(room1);
            myRooms[1] = room2.Name + ConnectedOrNot(room2);
            myRooms[2] = room3.Name + ConnectedOrNot(room3);
            myRooms[3] = room4.Name + ConnectedOrNot(room4);
            myRooms[4] = room5.Name + ConnectedOrNot(room5);

            listBox1.Items.AddRange(myRooms);
        }

        string ConnectedOrNot(room r)
        {
            Boolean b = r.Connected;

            if (b) return " (connected)";
            else return "";
        }

        private void ChatRoomForm_Load(object sender, EventArgs e)
        {
            refreshList();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;

            switch (index)
            {
                case 0:
                    selectedRoom = room1;
                    break;
                case 1:
                    selectedRoom = room2;
                    break;
                case 2:
                    selectedRoom = room3;
                    break;
                case 3:
                    selectedRoom = room4;
                    break;
                case 4:
                    selectedRoom = room5;
                    break;
            }

            // MessageBox.Show(Convert.ToString(index));

            if (!selectedRoom.Connected) connectButton.Text = "Connect";
            else connectButton.Text = "Disconnect";
        }
    }
}
