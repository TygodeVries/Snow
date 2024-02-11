using Snow.Containers;
using Snow.Formats;
using Snow.Level;
using Snow.Level.Entities;
using Snow.Network.Mappings;
using Snow.Network.Packets.Configuration.Clientbound;
using Snow.Network.Packets.Login.Clientbound;
using Snow.Network.Packets.Play.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snow.Network
{
    public class Connection
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


        public void SendAllEntitiesOfWorld(LevelSpace levelSpace)
        {
            foreach (Entity entity in levelSpace.GetAllEntities())
            {
                if(entity is Player)
                {
                    Player player = (Player)entity;
                    if(player.GetConnection() == this)
                    {
                        continue;
                    }
                }
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

        Player entityPlayer;

        public MinecraftServer minecraftServer;

        public Player GetEntityPlayer()
        {
            return entityPlayer;
        }


        public void Disconnect()
        {
            connected = false;
      //      entityPlayer.world.RemoveEntity(entityPlayer);
            minecraftServer.playerConnections.Remove(this);
            entityPlayer = null;
        }

        bool connected = true;


        TcpClient client;
        public Connection(TcpClient client, MinecraftServer minecraftServer)
        {
            this.client = client;
            this.minecraftServer = minecraftServer;
        }

        public void SendConnectionPackets(Player entityPlayer, string playerName)
        {
            this.entityPlayer = entityPlayer;   

            // #TODO Should wait for connection packet to see what the players name is.

            SendPacket(new LoginSuccess(entityPlayer.uuid, playerName));

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
            SendPacket(new GameEvent(0x0D, 0));
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

            SendPacket(new BlockUpdate(new Position(0, -3, 0), 1));
        }


        byte[] data = new byte[0];
        public void ReadPackets()
        {
            int available = client.Available;
            if (available > 0)
            {
                byte[] dataRead = new byte[available];
                client.GetStream().Read(dataRead, 0, dataRead.Length);

                data = data.Concat(dataRead).ToArray();
            }

            if(data.Length > 3)
            {
                int lenght = VarInt.FromByteArray(data, out int bytesRead);

                if(data.Length < lenght + bytesRead)
                {
                    return;
                }

                byte[] packet = new byte[lenght];
                Array.Copy(data, bytesRead, packet, 0, lenght);

                byte[] newData = new byte[data.Length - bytesRead - lenght];
                Array.Copy(data, bytesRead + lenght, newData, 0, data.Length - bytesRead - lenght);

                data = newData;

                HandlePacket(packet);
            }
        }

        void HandlePacket(byte[] packetData)
        {
            ServerboundPacket packet = MappingsManager.CreateServerboundPacket(packetData, this);

            if(packet == null)
            {
                // not implemented
                return;
            }

            packet.Use(this);
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

            throw new NotImplementedException();
     /*       Chunk chunk = new Chunk(x, z, world);
            SendPacket(new ChunkDataAndUpdateLight(chunk)); */
        }

        private ConnectionState currentState = ConnectionState.HANDSHAKE;

        public ConnectionState GetConnectionState()
        {
            return currentState;
        }

        public void SetConnectionState(ConnectionState state)
        {
            currentState = state;
        }

    }

    public enum ConnectionState
    {
        HANDSHAKE,
        LOGIN,
        CONFIGURATION,
        PLAY
    }
}
