using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class ChangeDifficultyPacket : ClientboundPacket
    {
        // 0: peaceful, 1: easy, 2: normal, 3: hard
        public byte Difficulty;

        public bool Locked;

        public ChangeDifficultyPacket(byte difficulty, bool locked)
        {
            Difficulty = difficulty;
            Locked = locked;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteByte(Difficulty);
            packetWriter.WriteBool(Locked);
        }
    }
}
