using Snow.Formats;
using Snow.Network.Packets.Configuration.Serverbound;
using Snow.Network.Packets.Login.Serverbound;
using Snow.Network.Packets.Play.Serverbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Entity
{
    internal class PlayerConnection
    {
        /// <summary>
        /// Send a packet to a player connection
        /// </summary>
        /// <param name="packet">The packet that will be send</param>
        public void SendPacket(ServerboundPacket packet)
        {
            // Create packet writer
            PacketWriter writer = new PacketWriter();

            // Create packet
            packet.Create(writer);
            byte[] bytes = writer.ToByteArray();

            // Calculate lenght
            byte[] lenghtBytes = VarInt.ToByteArray((uint) bytes.Length);

            // Add lenght and data bytes together and send it to client
            SendData(lenghtBytes.Concat(bytes).ToArray());
        }

        private void SendData(byte[] data)
        {
            client.GetStream().Write(data, 0, data.Length);
        }


        TcpClient client;
        public PlayerConnection(TcpClient client)
        {
            this.client = client;
        }

        public void SendConnectionPackets()
        {
            // Login
            SendPacket(new LoginSuccess());

            // Configuration
            SendPacket(new FeatureFlags());
            SendPacket(new RegistryData());
            SendPacket(new UpdateTags());
            SendPacket(new FinishConfiguration());

            // Play
            SendPacket(new Login());
            SendPacket(new ChangeDifficulty());
            SendPacket(new PlayerAbilities());
            SendPacket(new SetHeldItem());
            SendPacket(new UpdateRecipes());
            SendPacket(new EntityEvent());
            SendPacket(new Commands());
            SendPacket(new UpdateRecipeBook());
            SendPacket(new SynchronizePlayerPosition());
            SendPacket(new ServerData());
            SendPacket(new PlayerInfoUpdate());
            SendPacket(new InitalizeWorldBorder());
            SendPacket(new UpdateTime());
            SendPacket(new SetDefaultSpawnPosition());
            SendPacket(new GameEvent());
            SendPacket(new SetTickingState());
            SendPacket(new StepTick());
            SendPacket(new SetCenterChunk());
            SendPacket(new SetContainerContent());
            SendPacket(new SetEntityMetadata());
            SendPacket(new UpdateAttributes());
            SendPacket(new UpdateAdvancements());
            SendPacket(new SetHealth());
            SendPacket(new SetExperience());
            SendChunk();
            SendPacket(new UpdateTime());

            // Player should be spawned in now.
        }
    }
}
