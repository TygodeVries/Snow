using Snow.Entities;
using Snow.Events.Args;
using Snow.Formats;
using Snow.Levels;
using Snow.Network.Mappings;
using Snow.Network.Packets.Configuration.Clientbound;
using Snow.Network.Packets.Login.Clientbound;
using Snow.Network.Packets.Play.Clientbound;
using Snow.Servers;
using Snow.Worlds;
using System;
using System.Linq;
using System.Net.Sockets;

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

            packet.Create(writer);
            byte[] bytes = writer.ToByteArray();

            byte[] lenghtBytes = VarInt.ToByteArray((uint)bytes.Length);

            SendRawBytes(lenghtBytes.Concat(bytes).ToArray());

        }
        public void SendRawBytes(byte[] data)
        {
            try
            {
                client.GetStream().WriteAsync(data, 0, data.Length);
            }
            catch (Exception e)
            {
                    
            }
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

        internal void Connect(Server lobby, Player player)
        {
            this.entity = player;   

            SendPacket(new LoginSuccessPacket(player.GetUUID(), player.GetName()));

            SendPacket(new FeatureFlagsPacket());
            SendPacket(new RegistryDataPacket());
            SendPacket(new FinishConfigurationPacket());

            SendPacket(new LoginPacket(player));
            SendPacket(new ChangeDifficultyPacket(0x00, false));
            SendPacket(new PlayerAbilitiesPacket());
            SendPacket(new SetHeldItemPacket(0x00));
            SendPacket(new UpdateRecipesPacket());
            SendPacket(new Snow.Network.Packets.Play.Clientbound.CommandsPacket());
            SendPacket(new UpdateRecipeBookPacket());
            SendPacket(new SynchronizePlayerPositionPacket(0, 0, 0, 0, 0));
            SendPacket(new PlayerInfoUpdatePacket(0x00, player.GetUUID()));
            SendPacket(new InitializeWorldBorderPacket());
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

            SendRenderDistance(player.GetWorld(), 7);

            SendPacket(new UpdateTimePacket());
            SendPacket(new BlockUpdatePacket(new Position(0, -3, 0), 1));

            
        }

        public void SendRenderDistance(World world, int distance)
        {
            for(int x = -distance; x < distance; x++)
            {
                for (int z = -distance; z < distance; z++)
                {
                    Chunk chunk = world.GetChunk((x, z));
                    SendChunk(chunk);
                }
            }
        }

        public void SendChunk(Chunk chunk)
        {
            ChunkDataAndUpdateLightPacket packet = chunk.CreatePacket();
            SendPacket(packet);
        }


        byte[] data = new byte[0];
        internal void ReadPackets()
        {
            int available = client.Available;
            if (available > 0)
            {
                byte[] dataRead = new byte[available];
                client.GetStream().Read(dataRead, 0, dataRead.Length);

                data = data.Concat(dataRead).ToArray();
            }

            while(data.Length > 3)
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
        HANDSHAKE,
        LOGIN,
        CONFIGURATION,
        PLAY
    }
}
