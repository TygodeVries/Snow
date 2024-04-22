using Snow.Network.Packets.Status.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Status.Serverbound
{
    public class PingRequestPacket : ServerboundPacket
    {
        long payload;

        public override void Decode(PacketReader packetReader)
        {
            payload = packetReader.ReadLong();
        }

        public override void Use(Connection connection)
        {
            PingResponsePacket packet = new PingResponsePacket(payload);
            connection.SendPacket(packet);
        }
    }
}
