using Snow.Level.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Serverbound
{
    internal class Interact : ServerboundPacket
    {
        int entityId;
        int type;
        float targetX;
        float targetY;
        float targetZ;
        int hand;
        bool sneaking;

        public override void Decode(PacketReader packetReader)
        {
            entityId = packetReader.ReadVarInt();
            int type = packetReader.ReadVarInt();

            if(type == (int) InteractType.INTERACT_AT)
            {
                targetX = packetReader.ReadFloat();
                targetY = packetReader.ReadFloat();
                targetZ = packetReader.ReadFloat();
            }

            if(type == (int)InteractType.INTERACT || type == (int) InteractType.INTERACT_AT) 
            {
                hand = packetReader.ReadVarInt();
            }

            sneaking = packetReader.ReadBool();
        }

        public override void Use(Connection connection)
        {
            
        }

        enum InteractType
        {
            INTERACT,
            ATTACK,
            INTERACT_AT
        }
    }
}
