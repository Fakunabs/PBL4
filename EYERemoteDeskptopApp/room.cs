using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EYERemoteDeskptopApp
{
    public class room
    {
        private string name;
        private string address;
        private int port;
        private Boolean connected;
        private Connection connectionRoom;


        public room(string name, string address, int port)
        {
            this.name = name;
            this.address = address;
            this.port = port;
            this.connectionRoom = new Connection();
            this.Connected = false;
        }


        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
            }
        }


        public Boolean Connected
        {
            get
            {
                return connected;
            }
            set
            {
                connected = value;
            }
        }

        public Connection ConnectionRoom
        {
            get
            {
                return connectionRoom;
            }
            set
            {
                connectionRoom = value;
            }
        }

    }
}
