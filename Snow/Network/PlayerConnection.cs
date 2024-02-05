using Snow.Containers;
using Snow.Entities;
using Snow.Formats;
using Snow.Level;
using Snow.Network.Packets.Configuration.Clientbound;
using Snow.Network.Packets.Login.Clientbound;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snow.Network
{
    internal class PlayerConnection
    {
        /// <summary>
        /// Send a packet to a player connection
        /// </summary>
        /// <param name="packet">The packet that will be send</param>
        public void SendPacket(ClientboundPacket packet)
        {
            // Create packet writer
            PacketWriter writer = new PacketWriter();

            // Create packet
            packet.Create(writer);
            byte[] bytes = writer.ToByteArray();

            // Calculate lenght
            byte[] lenghtBytes = VarInt.ToByteArray((uint)bytes.Length);

            // Add lenght and data bytes together and send it to client
            SendData(lenghtBytes.Concat(bytes).ToArray());            
        }

        public void RegisterEntity(Entity entity)
        {
            SendPacket(new SpawnEntity(entity));
        }

        public void SendAllEntitiesOfWorld(World world)
        {
            foreach (Entity entity in world.GetAllEntities())
            {
                if(entity is EntityPlayer)
                {
                    EntityPlayer player = (EntityPlayer)entity;
                    if(player.connection == this)
                    {
                        continue;
                    }
                }

                RegisterEntity(entity);
            }
        }

        private void SendData(byte[] data)
        {
            if (connected)
            {
                try
                {
                    client.GetStream().Write(data, 0, data.Length);
                }
                catch (Exception e)
                {
                    Disconnect();
                }
            }
        }

        EntityPlayer entityPlayer;

        public MinecraftServer minecraftServer;

        public void Disconnect()
        {
            connected = false;
            entityPlayer.world.RemoveEntity(entityPlayer);
            minecraftServer.playerConnections.Remove(this);
            entityPlayer = null;
        }

        bool connected = true;


        TcpClient client;
        public PlayerConnection(TcpClient client, MinecraftServer minecraftServer)
        {
            this.client = client;
            this.minecraftServer = minecraftServer;
        }

        public void SendConnectionPackets(EntityPlayer entityPlayer)
        {
            this.entityPlayer = entityPlayer;   

            SendPacket(new LoginSuccess(entityPlayer.uuid, "TheSheepDev"));

            SendPacket(new FeatureFlags());
            SendPacket(new RegistryData());
            SendPacket(new FinishConfiguration());

            SendPacket(new Login(entityPlayer));
            SendPacket(new ChangeDifficulty(0x00, false));
            SendPacket(new PlayerAbilities());
            SendPacket(new SetHeldItem(0x00));
            SendPacket(new UpdateRecipes());
            SendPacket(new Commands());
            SendPacket(new UpdateRecipeBook());
            SendPacket(new SynchronizePlayerPosition(0, 0, 0, 0, 0));
            SendPacket(new PlayerInfoUpdate(0x00, entityPlayer.uuid));
            SendPacket(new InitializeWorldBorder());
            SendPacket(new UpdateTime());
            SendPacket(new SetDefaultSpawnPosition());
            SendPacket(new GameEvent());
            SendPacket(new SetTickingState());
            SendPacket(new StepTick());
            SendPacket(new SetCenterChunk(0, 0));

            Inventory inventory = new Inventory(44);

            ItemStack block = new ItemStack();
            block.present = true;

            inventory.SetItem(40, block);
       //     SendPacket(new SetEntityMetadata());
            SendPacket(new UpdateAttributes());
            SendPacket(new UpdateAdvancements());
            SendPacket(new SetHealth());
            SendPacket(new SetExperience(0, 0, 0));

            SendSpiralChunks();
            SendPacket(new UpdateTime());

            SendPacket(new BlockUpdate(new Position(0, 0, 0), 1));
        }

        public void SendSpiralChunks()
        {
            World world = new World();

            int radius = 5;
            int centerX = 0;
            int centerZ = 0;

            SendPacket(new ChunkBatchStart());

            int chunksSent = 0;
            for (int x = centerX - radius; x <= centerX + radius; x++)
            {
                for (int z = centerZ - radius; z <= centerZ + radius; z++)
                {
                    SendChunkData(x, z, world);
                    chunksSent++; // Increment the counter
                }
            }


            SendPacket(new ChunkBatchFinished(chunksSent));
        }

        public void SendChunkData(int x, int z, World world)
        {
            Chunk chunk = new Chunk(x, z, world);
            SendPacket(new ChunkDataAndUpdateLight(chunk));
        }

    }
}
