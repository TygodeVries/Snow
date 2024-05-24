using Snow.Events.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Serverbound
{
    internal class UseItemPacket : ServerboundPacket
    {
        int hand;

        public override void Decode(PacketReader packetReader)
        {
            hand = packetReader.ReadVarInt();
        }

        public override void Use(Connection connection)
        {
            connection.GetPlayer().OnRightClick.Invoke(connection.GetPlayer(), new OnRightClickArgs(connection.GetPlayer()));
        }
    }
}
