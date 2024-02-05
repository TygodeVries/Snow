using Snow.Formats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network
{
    internal class ServerboundPacketMappings
    {


        static Dictionary<int, string> mappings = new Dictionary<int, string>();

        public static void Load()
        {
            Console.WriteLine("Loading serverbound packet mappings...");
            string[] lines = File.ReadAllLines(@"Data/serverbound.mappings");
        }

        public static ServerboundPacket CreateNewPacket(byte[] packet)
        {
            int read = 0;
            int id = VarInt.FromByteArray(packet, out read);

            byte[] packetContent = new byte[packet.Length - read];
            Array.Copy(packet, read, packetContent, 0, packet.Length - read);
            ServerboundPacket serverboundPacket = (ServerboundPacket) Activator.CreateInstance(mappings[id], "Snow.Network.Packets.Play.Serverbound").Unwrap();

            PacketReader packetReader = new PacketReader(packetContent);
            serverboundPacket.Decode(packetReader);

            return serverboundPacket;
        }
    }
}
