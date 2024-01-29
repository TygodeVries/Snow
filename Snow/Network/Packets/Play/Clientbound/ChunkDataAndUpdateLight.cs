using Snow.Formats.Nbt;
using Snow.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    internal class ChunkDataAndUpdateLight : ClientboundPacket
    {
        Chunk chunk;

        public ChunkDataAndUpdateLight(Chunk chunk)
        {
            this.chunk = chunk;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x25);

            packetWriter.WriteInt(chunk.x);
            packetWriter.WriteInt(chunk.z);

            NbtCompoundTag heightmapsCompoundTag = chunk.GetHeightmaps();
            packetWriter.WriteCompoundTag(heightmapsCompoundTag);
        }
    }
}
