using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    [Serializable]
    public class Packet
    {
        public PacketType type = PacketType.EMPTY;
    }
}
