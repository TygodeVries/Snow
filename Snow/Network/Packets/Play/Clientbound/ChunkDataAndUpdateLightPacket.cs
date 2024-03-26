using Snow.Formats.Nbt;
using System.Collections;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class ChunkDataAndUpdateLightPacket : ClientboundPacket
    {
        public int x;
        public int z;

        NbtCompoundTag heightmap;

        byte[] data;

        public ChunkDataAndUpdateLightPacket(int x, int z, NbtCompoundTag heightmap, byte[] data)
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


            /*
             *  Masks
             */
            BitArray skyLight = new BitArray(24 + 2);
            skyLight.SetAll(true);
            packetWriter.WriteBitArray(skyLight);


            // Copy for blocks
            packetWriter.WriteBitArray(skyLight);


            BitArray skyLightEmpty = new BitArray(24 + 2);
            skyLightEmpty.SetAll(false);
            packetWriter.WriteBitArray(skyLightEmpty);


            // Copy for blocks
            packetWriter.WriteBitArray(skyLightEmpty);

            /*
             * Light
             */
            packetWriter.WriteVarInt(24 + 2);
            for (int i = 0; i < 24 + 2; i++)
            {
                packetWriter.WriteVarInt(2048);
                byte[] bytes = new byte[2048];
                for (int j = 0; j < 2048; j++)
                    bytes[j] = (byte)0xFF;
                packetWriter.WriteByteArray(bytes);
            }


            // Copy for blocks
            packetWriter.WriteVarInt(24 + 2);
            for (int i = 0; i < 24 + 2; i++)
            {
                packetWriter.WriteVarInt(2048);
                byte[] bytes = new byte[2048];
                for (int j = 0; j < 2048; j++)
                    bytes[j] = (byte)0xFF;
                packetWriter.WriteByteArray(bytes);
            }
        }
    }
}
