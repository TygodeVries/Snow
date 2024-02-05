using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network
{
    public abstract class ClientboundPacket
    {
        public abstract void Create(PacketWriter packetWriter);
    }
}
