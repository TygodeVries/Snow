using Snow.Entities;
using Snow.Formats;
using Snow.Formats.Nbt;
using Snow.Levels;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snow.Worlds
{
    public class Chunk
    {
        World world;

        public int x;
        public int z;

        public Chunk(World world, int x, int z)
        {
            this.world = world;
            data = new BlockType[16 * world.GetWorldHeight() * 16];
            this.x = x;
            this.z = z;
        }

        public BlockType[] data;

        public void SetBlockAt(Position position, BlockType blockType)
        {
            int x = position.x;
            int y = position.y;
            int z = position.z;

            int Width = 16;
            int Depth = 16;

            int index = x + (z * Width) + (y * Width * Depth);

            data[index] = blockType;
        }

        public BlockType GetBlockAt(Position position)
        {
            int x = position.x;
            int y = position.y;
            int z = position.z;

            int Width = 16;
            int Depth = 16;

            int index = x + (z * Width) + (y * Width * Depth);

            return data[index];
        }


        public ChunkDataAndUpdateLightPacket CreatePacket()
        {
            NbtCompoundTag heightmap = new NbtCompoundTag();

            NbtLongArrayTag MOTION_BLOCKING = new NbtLongArrayTag();
            NbtLongArrayTag WORLD_SURFACE = new NbtLongArrayTag();
            MOTION_BLOCKING.values = new long[37];
            WORLD_SURFACE.values = new long[37];
            heightmap.AddField("MOTION_BLOCKING", MOTION_BLOCKING);
            heightmap.AddField("WORLD_SURFACE", WORLD_SURFACE);

            ChunkDataAndUpdateLightPacket packet = new ChunkDataAndUpdateLightPacket(x, z, heightmap, GenerateChunkData());
            return packet;
        }

        byte[] GenerateChunkData()
        {
            List<byte> finalDataList = new List<byte>();

            for (int i = 0; i < world.GetWorldHeight() / 16; i++)
            {
                short blockCount = 0;
                List<BlockType> pallet = new List<BlockType>();

                byte[] data = new byte[16 * 16 * 16];
                int index = 0;

                for (int y = 0; y < 16; y++)
                    for (int z = 0; z < 16; z++)
                        for (int x = 0; x < 16; x++)
                        {
                            BlockType blockData = GetBlockAt(new Position(x, y + (i * 16), z));
                            if (blockData != BlockType.AIR)
                            {
                                blockCount++;
                            }

                            if (!pallet.Contains(blockData))
                            {
                                pallet.Add(blockData);
                            }

                            data[index++] = (byte)pallet.IndexOf(blockData);
                        }

                byte[] palletData = VarInt.ToByteArray((uint)pallet.Count);
                for (int m = 0; m < pallet.Count; m++)
                {
                    palletData = palletData.Concat(VarInt.ToByteArray((uint)pallet[m])).ToArray();
                }

                byte[] bitsPerBlock = new byte[] { 0x08 };

                byte[] blockCountData = BitConverter.GetBytes(blockCount);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(blockCountData);

                byte[] sectionData = blockCountData.Concat(bitsPerBlock).Concat(palletData)
                                    .Concat(VarInt.ToByteArray((uint)(data.Length / 8))).Concat(data).ToArray();

                finalDataList.AddRange(sectionData);
                finalDataList.AddRange(new byte[] { 0, 0, 0 });
            }

            return finalDataList.ToArray();
        }

    }
}
