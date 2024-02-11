using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network
{
    public abstract class ServerboundPacket
    {
        public abstract void Decode(PacketReader packetReader);

        public abstract void Use(Connection connection);
    }
}
