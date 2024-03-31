using Snow.Entities;
using Snow.Formats;
using Snow.Formats.Nbt;
using Snow.Levels;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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
            this.x = x;
            this.z = z;

            int cs = (int) Math.Floor(world.GetWorldHeight() / 16d);
            
            chunkSections = new ChunkSection[cs];

            for(int i = 0; i < cs; i++)
            {
                chunkSections[i] = new SolidChunkSection(0);
            }
        }

        ChunkSection[] chunkSections;


        public BlockType GetBlockAt(Position position)
        {
            try
            {
                int cs = (int)Math.Floor(position.y / 16d);

                BlockType blockType = chunkSections[cs].Get(position.x % 16, position.y % 16, position.z % 16);
                return blockType;
            }  catch(Exception e)
            {
                Log.Err($"Failed to get block at {position.ToString()} because: " + e);
            }

            return 0;
        }

        public void Optimize()
        {
            for (int i = 0; i < chunkSections.Length; i++)
            {
                ChunkSection section = chunkSections[i];

                if(section.GetType() == typeof(DetailedChunkSection))
                {
                    ((DetailedChunkSection)section).Optimize();
                }
            }
        }

        public void SetBlockAt(Position position, BlockType blockType)
        {
            try
            {
                int x = position.x;
                int y = position.y;
                int z = position.z;

                int chunkSection = (int)Math.Floor(position.y / 16d);
                ChunkSection section = chunkSections[chunkSection];

                if (section.GetType() == typeof(DetailedChunkSection))
                {
                    ((DetailedChunkSection)section).Set(blockType, x, y % 16, z);
                    return;
                }
                else if (section.GetType() == typeof(SolidChunkSection))
                {
                    SolidChunkSection detailedChunkSection = (SolidChunkSection)section;
                    if (detailedChunkSection.Get(0, 0, 0) != blockType)
                    {
                        BlockType bt = detailedChunkSection.Get(0, 0, 0);
                        DetailedChunkSection detailedChunk = new DetailedChunkSection();
                        for (int a = 0; a < 16; a++)
                        {
                            for (int b = 0; b < 16; b++)
                            {
                                for (int c = 0; c < 16; c++)
                                {
                                    detailedChunk.Set(bt, a, b, c);
                                }
                            }
                        }

                        detailedChunk.Set(blockType, x, y % 16, z);

                        chunkSections[chunkSection] = detailedChunk;
                    }
                }
                else
                {
                    Log.Err("Invalid or uninemplemented sectionchunktype! type: " + section.GetType());
                }
            } catch(Exception e)
            {
                Log.Err($"Failed to setblock at {position.ToString()} because: " + e);
            }
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
