using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network
{
    internal abstract class ServerboundPacket
    {
        public abstract void Decode(PacketReader packetReader);
    }
}
