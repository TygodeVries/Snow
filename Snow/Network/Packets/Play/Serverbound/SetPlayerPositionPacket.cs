using Snow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Serverbound
{
    public class SetPlayerPositionPacket : ServerboundPacket
    {
        double x;
        double y;
        double z;

        bool onGround;

        public override void Decode(PacketReader packetReader)
        {
            x = packetReader.ReadDouble();
            y = packetReader.ReadDouble();
            z = packetReader.ReadDouble();

            onGround = packetReader.ReadBool();
        }

        public override void Use(Connection connection)
        {
            Player player = connection.GetPlayer();
            player.Teleport(connection.GetPlayer().GetWorld(), x, y, z, player.GetYaw(), player.GetPitch());;
        }
    }
}
