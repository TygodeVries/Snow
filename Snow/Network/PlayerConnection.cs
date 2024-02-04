using Snow.Containers;
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

            Console.WriteLine("Sending packet: " + packet.GetType().Name.Split('.').Last() + " lenght: " + bytes.Length);

            // Add lenght and data bytes together and send it to client
            SendData(lenghtBytes.Concat(bytes).ToArray());

            Thread.Sleep(5);
            
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

            SendPacket(new LoginSuccess(playerUUID, "TheSheepDev"));

            SendPacket(new FeatureFlags());
            SendPacket(new RegistryData());
            SendPacket(new FinishConfiguration());

            SendPacket(new Login());
            SendPacket(new ChangeDifficulty(0x00, false));
            SendPacket(new PlayerAbilities());
            SendPacket(new SetHeldItem(0x00));
            SendPacket(new UpdateRecipes());
            SendPacket(new Commands());
            SendPacket(new UpdateRecipeBook());
            SendPacket(new SynchronizePlayerPosition(0, 0, 0, 0, 0));
            SendPacket(new PlayerInfoUpdate(0x00, playerUUID));
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

            Thread.Sleep(1000);

            SendSpiralChunks();
            SendPacket(new UpdateTime());

            SendPacket(new BlockUpdate(new Position(0, 0, 0), 1));

            // Player should be spawned in now.

            while(true)
            {
                SendPacket(new UpdateTime());
                Thread.Sleep(1000);
            }
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
