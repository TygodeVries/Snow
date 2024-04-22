using Snow.Entities;
using Snow.Events;
using Snow.Formats;
using Snow.Levels;
using Snow.Network.Mappings;
using Snow.Network.Packets.Configuration.Clientbound;
using Snow.Network.Packets.Login.Clientbound;
using Snow.Network.Packets.Play.Clientbound;
using Snow.Servers;
using Snow.Worlds;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Snow.Network
{
    public class Connection
    {
        private ConnectionState currentState = ConnectionState.HANDSHAKE;
        public ConnectionState GetConnectionState()
        {
            return currentState;
        }
        public void SetConnectionState(ConnectionState state)
        {
            currentState = state;
        }

        TcpClient client;
        public Connection(Server server, TcpClient client)
        {
            this.server = server;
            this.client = client;
        }

        public void SendPacket(ClientboundPacket packet)
        {

            PacketWriter writer = new PacketWriter();
            MemoryStream stream = new MemoryStream();

            packet.Create(writer);
            byte[] bytes = writer.ToByteArray();

            byte[] lenghtBytes = VarInt.ToByteArray((uint)bytes.Length);

            stream.Write(lenghtBytes, 0, lenghtBytes.Length);
            stream.Write(bytes, 0, bytes.Length);

            SendRawBytes(stream.ToArray());
        }
        public void SendRawBytes(byte[] data)
        {
            if (!connected)
                return;

            try
            {
                client.GetStream().WriteAsync(data, 0, data.Length);
            }
            catch (Exception e)
            {
                Disconnected();
            }
        }
        public async Task SendRawBytesAsync(byte[] data)
        {
            if (!connected)
                return;

            try
            {
                client.GetStream().WriteAsync(data, 0, data.Length);
            }
            catch (Exception e)
            {
                Disconnected();
            }
        }

        public async Task SendPacketAsync(ClientboundPacket packet)
        {
            PacketWriter writer = new PacketWriter();

            packet.Create(writer);
            byte[] bytes = writer.ToByteArray();

            byte[] lengthBytes = VarInt.ToByteArray((uint)bytes.Length);

            await SendRawBytesAsync(lengthBytes.Concat(bytes).ToArray());
        }

        private Player entity = null;
        public Player GetPlayer()
        {
            return entity;
        }

        private Server server;
        public Server GetServer()
        {
            return server;
        }

        internal void Connect(Server server, Player player)
        {
            this.server = server;
            this.entity = player;   

            SendPacket(new LoginSuccessPacket(player.GetUUID(), player.GetName()));

            SendPacket(new FeatureFlagsPacket());
            SendPacket(new RegistryDataPacket());
            SendPacket(new FinishConfigurationPacket());

            SendPacket(new LoginPacket(player));
            SendPacket(new ChangeDifficultyPacket(0x00, false));
            SendPacket(new PlayerAbilitiesPacket(0x02, 0, 0.1f));
            SendPacket(new SetHeldItemPacket(0x00));
            SendPacket(new UpdateRecipesPacket());
            SendPacket(new Snow.Network.Packets.Play.Clientbound.CommandsPacket());
            SendPacket(new UpdateRecipeBookPacket());
            SendPacket(new SynchronizePlayerPositionPacket(0, 100, 0, 0, 0));

            PlayerInfoUpdatePacket playerInfoUpdatePacket = new PlayerInfoUpdatePacket(player.GetUUID());
            playerInfoUpdatePacket.SetAddPlayerPayload(player.GetName());
            SendPacket(playerInfoUpdatePacket);

            SendPacket(new InitializeWorldBorderPacket(0, 0, 1, 1, 1, 0, 0));
            SendPacket(new UpdateTimePacket());
            SendPacket(new SetDefaultSpawnPositionPacket());
            SendPacket(new GameEventPacket(0x0D, 0));
            SendPacket(new SetTickingStatePacket());
            SendPacket(new StepTickPacket());
            SendPacket(new SetCenterChunkPacket(0, 0));
            SendPacket(new UpdateAttributesPacket());
            SendPacket(new UpdateAdvancementsPacket());
            SendPacket(new SetHealthPacket());
            SendPacket(new SetExperiencePacket(0, 0, 0));

            SendChunk((0, 0));
            SendPacket(new InitializeWorldBorderPacket(0, 0, 10000, server.GetSettings().GetDouble("world-border"), 1, 1, 5));

            SendPacket(new UpdateTimePacket());
            SendPacket(new BlockUpdatePacket(new Position(0, -3, 0), 1));

            if(connected)
            {
                SendRenderDistance(player.GetWorld(), 7, new Vector3(player.GetPosistion().x / 16, 0, player.GetPosistion().z / 16));
                SendPacket(new PlayerAbilitiesPacket(0x00, 0.05f, 0.1f));
                player.SetFlying(true);
                player.SetAllowFlying(true);
            }

        }

        public void Flush()
        {
            if(client.Connected)
                client.GetStream().Flush();
        }


        private async Task SendChunksAsync(List<(int, int)> chunks)
        {
            foreach (var chunk in chunks)
            {
                await Task.Run(() => SendChunk(chunk));
            }
        }

        public void SendRenderDistance(World world, int distance, Vector3 centerChunk)
        {
            List<(int, int)> chunksToSend = new List<(int, int)>();

            // Calculate distances for all chunks
            for (int x = -distance; x < distance; x++)
            {
                for (int z = -distance; z < distance; z++)
                {
                    (int, int) location = (x + ((int)centerChunk.x), z + ((int)centerChunk.z));
                    chunksToSend.Add(location);
                }
            }

            // Sort chunks based on distance from the center chunk
            chunksToSend.Sort((a, b) => DistanceSquared(a, centerChunk).CompareTo(DistanceSquared(b, centerChunk)));

            // Send chunks in sorted order
            SendChunksAsync(chunksToSend);
        }


        // Function to calculate squared distance between two chunks
        private double DistanceSquared((int, int) chunkA, Vector3 chunkB)
        {
            return (chunkA.Item1 - chunkB.x) * (chunkA.Item1 - chunkB.x) + (chunkA.Item2 - chunkB.z) * (chunkA.Item2 - chunkB.z);
        }

        private List<(int, int)> clientLoadedChunks = new List<(int, int)>();

        private void AddClientLoadedChunk((int, int) a)
        {
            clientLoadedChunks.Add(a);
        }

        public bool HasLoadedChunkClientside((int, int) a)
        {
            return clientLoadedChunks.Contains(a);
        }


        public async void SendChunk((int, int) location)
        {
            // Check if chunk is already loaded
            if (HasLoadedChunkClientside(location))
                return;

            // Add chunk to loaded chunks
            AddClientLoadedChunk(location);

            // Get player and world
            Player player = GetPlayer();
            World world = player.GetWorld();

            // Get the chunk
            Chunk chunk = await world.GetChunkAsync(location);

            if(chunk == null)
            {
                // Failed to laod chunk?
                return;
            }

            // Create packet
            ChunkDataAndUpdateLightPacket packet = chunk.CreatePacket();
           
            // Send packet
            SendPacketAsync(packet);
        }

        private bool connected = true; 
        public bool IsConnected()
        {
            return connected;
        }

        public void Disconnected()
        {
            if (!connected || GetPlayer() == null)
                return;
            connected = false;
            client.Close();
            Log.Send($"{GetPlayer().GetName()} disconnected.");
            GetServer().BroadcastMessage(new TextComponent($"{GetPlayer().GetName()} disconnected."));
            GetPlayer().Remove();
        }

        byte[] data = new byte[0];
        internal void ReadPackets()
        {
            if(!connected)
                return;

            int available = client.Available;
            if (available > 0)
            {
                if (client.Connected)
                {
                    byte[] dataRead = new byte[available];
                    client.GetStream().Read(dataRead, 0, dataRead.Length);

                    data = data.Concat(dataRead).ToArray();
                }
                else
                {
                    Disconnected();
                }
            }

            // read up to 10 packets
            for (int i = 0; i < 10; i++)
                if (data.Length > 3)
                {
                    int lenght = VarInt.FromByteArray(data, out int bytesRead);

                    if (data.Length < lenght + bytesRead)
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
        internal void HandlePacket(byte[] packetData)
        {
            ServerboundPacket packet = MappingsManager.CreateServerboundPacket(packetData, this);

            if(packet == null)
            {
                // not implemented
                return;
            }

            packet.Use(this);
        }
    }

    public enum ConnectionState
    {
        STATUS,
        HANDSHAKE,
        LOGIN,
        CONFIGURATION,
        PLAY
    }
}
