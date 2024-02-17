using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Serverbound
{
    public class SetPlayerPositionandRotation : ServerboundPacket
    {
        double x;
        double y;
        double z;

        float yaw;
        float pitch;

        bool onGround;

        public override void Decode(PacketReader packetReader)
        {
            x = packetReader.ReadDouble();
            y = packetReader.ReadDouble();
            z = packetReader.ReadDouble();

            yaw = packetReader.ReadFloat();
            pitch = packetReader.ReadFloat();

            onGround = packetReader.ReadBool();
        }

        public override void Use(Connection connection)
        {
            connection.GetEntity().Teleport(x, y, z);
        }
    }
}
