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

namespace Snow.Network.Entity
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

            Console.WriteLine("Sending packet: " + packet.GetType().Name.Split('.').Last() + " lenght: " + bytes.Length);

            // Add lenght and data bytes together and send it to client
            SendData(lenghtBytes.Concat(bytes).ToArray());

            Thread.Sleep(1000);
            
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
            UUID playerUUID = UUID.Random();

            // Login
            SendPacket(new LoginSuccess(playerUUID, "TheSheepDev")); // Done

            // Configuration
            SendPacket(new FeatureFlags());
            SendPacket(new RegistryData());
        //    SendPacket(new UpdateTags());
            SendPacket(new FinishConfiguration());

            SendPacket(new Login());
            SendPacket(new ChangeDifficulty(0x00, false)); // Done
            SendPacket(new PlayerAbilities());
            SendPacket(new SetHeldItem(0x00));
            SendPacket(new UpdateRecipes());
       //     SendPacket(new EntityEvent());
            SendPacket(new Commands());
            SendPacket(new UpdateRecipeBook());
            SendPacket(new SynchronizePlayerPosition(0, 0, 0, 0, 0));
       //     SendPacket(new ServerData());
            SendPacket(new PlayerInfoUpdate(0x00, playerUUID));
            SendPacket(new InitializeWorldBorder());
            SendPacket(new UpdateTime());
            SendPacket(new SetDefaultSpawnPosition());
            SendPacket(new GameEvent());
            SendPacket(new SetTickingState());
            SendPacket(new StepTick());
            SendPacket(new SetCenterChunk(0, 0));
            SendPacket(new SetContainerContent());
       //     SendPacket(new SetEntityMetadata());
            SendPacket(new UpdateAttributes());
            SendPacket(new UpdateAdvancements());
            SendPacket(new SetHealth());
            SendPacket(new SetExperience(0, 0, 0));
            SendPacket(new ChunkBatchStart());
            SendTestChunks();
            SendPacket(new ChunkBatchFinished());
            SendPacket(new UpdateTime());

            // Player should be spawned in now.
        }

        public void SendTestChunks()
        {
            World world = new World();

            for (int x = -5; x < 5; x++)
            {
                for (int z = -5; z < 5; z++)
                {
                    Chunk chunk = new Chunk(x, z, world);
                    SendPacket(new ChunkDataAndUpdateLight(chunk));
                }
            }
        }
    }
}
