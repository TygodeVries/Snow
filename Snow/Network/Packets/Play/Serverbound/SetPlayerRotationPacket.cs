using Snow.Entities;
using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Serverbound
{
    internal class SetPlayerRotationPacket : ServerboundPacket
    {
        float yaw;
        float pitch;
        bool onGround;

        public override void Decode(PacketReader packetReader)
        {
            yaw = packetReader.ReadFloat();
            pitch = packetReader.ReadFloat();

            onGround = packetReader.ReadBool();
        }

        public override void Use(Connection connection)
        {
            Player player = connection.GetPlayer();
            Vector3 position = player.GetPosistion();

            player.Teleport(connection.GetPlayer().GetWorld(), position.x, position.y, position.z, yaw, pitch);
        }
    }
}
