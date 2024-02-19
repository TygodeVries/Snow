using Snow.Entities;
using Snow.Events.Args;
using Snow.Formats;
using Snow.Levels;
using Snow.Network.Mappings;
using Snow.Network.Packets.Configuration.Clientbound;
using Snow.Network.Packets.Login.Clientbound;
using Snow.Network.Packets.Play.Clientbound;
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
        public Connection(Lobby lobby, TcpClient client)
        {
            this.lobby = lobby;
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
                client.GetStream().Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                    
            }
        }
        public void SendLevel(Level level)
        {
            for(int x = -7; x < 7; x++)
            {
                for (int z = -7; z < 7; z++)
                {
                    level.SendChunkToConnection(this, x, z);
                }
            }
        }
        
        private Player entity = null;
        public Player GetPlayer()
        {
            return entity;
        }

        private Lobby lobby;
        public Lobby GetLobby()
        {
            return lobby;
        }

        internal void Connect(Lobby lobby, Player player)
        {
            this.entity = player;   

            SendPacket(new LoginSuccess(player.GetUUID(), player.GetName()));

            SendPacket(new FeatureFlags());
            SendPacket(new RegistryData());
            SendPacket(new FinishConfiguration());

            SendPacket(new Login(player));
            SendPacket(new ChangeDifficulty(0x00, false));
            SendPacket(new PlayerAbilities());
            SendPacket(new SetHeldItem(0x00));
            SendPacket(new UpdateRecipes());
            SendPacket(new Snow.Network.Packets.Play.Clientbound.Commands());
            SendPacket(new UpdateRecipeBook());
            SendPacket(new SynchronizePlayerPosition(0, 0, 0, 0, 0));
            SendPacket(new PlayerInfoUpdate(0x00, player.GetUUID()));
            SendPacket(new InitializeWorldBorder());
            SendPacket(new UpdateTime());
            SendPacket(new SetDefaultSpawnPosition());
            SendPacket(new GameEvent(0x0D, 0));
            SendPacket(new SetTickingState());
            SendPacket(new StepTick());
            SendPacket(new SetCenterChunk(0, 0));
            SendPacket(new UpdateAttributes());
            SendPacket(new UpdateAdvancements());
            SendPacket(new SetHealth());
            SendPacket(new SetExperience(0, 0, 0));
            SendLevel(lobby.GetCurrentLevel());
            SendPacket(new UpdateTime());
            SendPacket(new BlockUpdate(new Position(0, -3, 0), 1));

            
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
