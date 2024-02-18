using Snow.Formats.Nbt;
using Snow.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class ChunkDataAndUpdateLight : ClientboundPacket
    {
        public int x;
        public int z;

        NbtCompoundTag heightmap;

        byte[] data;

        public ChunkDataAndUpdateLight(int x, int z, NbtCompoundTag heightmap, byte[] data)
        {
            this.x = x;
            this.z = z;
            this.heightmap = heightmap;
            this.data = data;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteInt(x);
            packetWriter.WriteInt(z);

            packetWriter.WriteCompoundTag(heightmap);

            packetWriter.WriteVarInt(data.Length);
            packetWriter.WriteByteArray(data);

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
