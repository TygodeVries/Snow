using Snow.Entities;
using Snow.Formats;
using Snow.Formats.Nbt;
using Snow.Levels;
using Snow.Network;
using Snow.Network.Packets.Play.Clientbound;
using Snow.Worlds.Blocks;
using System;
using System.Collections.Generic;
using System.IO;

namespace Snow.Worlds
{
    public class Chunk
    {
        World world;
        public World GetWorld()
        {
            return world;
        }



        public int x;
        public int z;

        private List<ProgrammableBlock> programmableBlocks = new List<ProgrammableBlock>();

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

        /// <summary>
        /// Broadcast a packet to everyone who has this chunk loaded.
        /// </summary>
        public void BroadcastPacket(ClientboundPacket packet)
        {
            List<Player> players = GetWorld().GetPlayers();

            foreach(Player player in players)
            {
                // Check if player has chunk loaded.
                if (player.GetConnection().HasLoadedChunkClientside((x, z)))
                {
                    player.GetConnection().SendPacket(packet);
                }
            }
        }

        ChunkSection[] chunkSections;

        public void TickProgrammableBlocks()
        {
            foreach(ProgrammableBlock programmableBlock in programmableBlocks)
            {
                programmableBlock.Tick();
            }
        }

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
            lock (chunkSections)
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
                }
                catch (Exception e)
                {
                    Log.Err($"Failed to setblock at {position.ToString()} because: " + e);
                }
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
            MemoryStream memStream = new MemoryStream();
    
            for (int i = 0; i < world.GetWorldHeight() / 16; i++)
            {

                if(chunkSections[i].GetType() == typeof(SolidChunkSection))
                {
                    WriteFullChunkSection(memStream, i);
                }
                else {
                    WriteDetailedChunkSection(memStream, i);
                }

                // Write biome data

                // BPE = 0
                memStream.WriteByte(0x00);

                // Write Type
                byte[] biomeType = VarInt.ToByteArray((uint) 0);
                memStream.Write(biomeType, 0, biomeType.Length);

                // Write data lenght (is always 0)
                byte[] dataLenght = VarInt.ToByteArray((uint)0);
                memStream.Write(dataLenght, 0, dataLenght.Length);
            }

            return memStream.ToArray();
        }

        private void WriteFullChunkSection(MemoryStream memStream, int i)
        {
            // Write blockcount
            byte[] blockCountData = BitConverter.GetBytes((short) 9999);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(blockCountData);
            memStream.Write(blockCountData, 0, blockCountData.Length);


            // Write bits per block
            memStream.WriteByte(0x00);

            // Write block id
            byte[] id = VarInt.ToByteArray((uint)GetBlockAt(new Position(0, (i * 16), 0)));
            memStream.Write(id, 0, id.Length);

            // Write data lenght
            byte[] dataLenght = VarInt.ToByteArray((uint)0);
            memStream.Write(dataLenght, 0, dataLenght.Length);
        }

        private void WriteDetailedChunkSection(MemoryStream memStream, int i)
        {
            short blockCount = 0;
            List<BlockType> pallet = new List<BlockType>();

            byte[] data = new byte[16 * 16 * 16];

            int index = 0;
            // Get chunk data.
            for (int y = 0; y < 16; y++)
                for (int z = 0; z < 16; z++)
                    for (int x = 0; x < 16; x++)
                    {
                        //#TODO
                        // Bad fix, do correct later!
                        int realX = Math.Abs(((x + 8) % 16) - 15);

                        BlockType blockData = GetBlockAt(new Position(realX, y + (i * 16), z));
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

            // Write blockcount
            byte[] blockCountData = BitConverter.GetBytes(blockCount);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(blockCountData);

            memStream.Write(blockCountData, 0, blockCountData.Length);

            // Write bits per block
            byte[] bitsPerBlock = new byte[] { 0x08 };
            memStream.Write(bitsPerBlock, 0, bitsPerBlock.Length);

            // Write pallet data lenght
            byte[] palletLenght = VarInt.ToByteArray((uint)pallet.Count);
            memStream.Write(palletLenght, 0, palletLenght.Length);

            // Write pallet data
            for (int m = 0; m < pallet.Count; m++)
            {
                byte[] a = VarInt.ToByteArray((uint)pallet[m]);
                memStream.Write(a, 0, a.Length);
            }

            // Write data lenght
            byte[] b = VarInt.ToByteArray((uint)(data.Length / 8));
            memStream.Write(b, 0, b.Length);

            // Write data
            memStream.Write(data, 0, data.Length);
        }

    }
}
