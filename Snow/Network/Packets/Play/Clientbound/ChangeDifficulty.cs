using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    internal class ChangeDifficulty : ClientboundPacket
    {
        // 0: peaceful, 1: easy, 2: normal, 3: hard
        public byte Difficulty;

        public bool Locked;

        public ChangeDifficulty(byte difficulty, bool locked)
        {
            Difficulty = difficulty;
            Locked = locked;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x0B);

            packetWriter.WriteByte(Difficulty);
            packetWriter.WriteBool(Locked);
        }
    }
}
