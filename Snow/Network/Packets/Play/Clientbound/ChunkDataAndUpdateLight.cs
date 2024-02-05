using Snow.Formats.Nbt;
using Snow.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class ChunkDataAndUpdateLight : ClientboundPacket
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

            byte[] chunkData = chunk.GetChunkData();
            packetWriter.WriteVarInt(chunkData.Length);
            packetWriter.WriteByteArray(chunkData);

            int BLOCK_ENTITY_COUNT = 0;
            packetWriter.WriteVarInt(BLOCK_ENTITY_COUNT);

            byte[] light_flags = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            packetWriter.WriteByteArray(light_flags);

            int SKYLIGHT = 0;
            int BLOCKLIGHT = 0;
            packetWriter.WriteVarInt(SKYLIGHT);
            packetWriter.WriteVarInt(BLOCKLIGHT);
        }
    }
}
