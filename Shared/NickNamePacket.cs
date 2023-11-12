using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{

    [Serializable]
    public class NickNamePacket : Packet
    {
        public string nickName = String.Empty;
        public NickNamePacket(string nickName)
        {
            this.type = PacketType.NICKNAME;
            this.nickName = nickName;
        }
    }
}
