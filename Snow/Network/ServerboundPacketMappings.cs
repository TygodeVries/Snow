using Snow.Formats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network
{
    public class ServerboundPacketMappings
    {


        static Dictionary<int, Type> mappings = new Dictionary<int, Type>();

        public static void Load()
        {
            mappings.Clear();

            try
            {
                Console.WriteLine("Loading serverbound packet mappings...");
                string[] lines = File.ReadAllLines(@"Data/serverbound.mappings");

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] args = lines[i].Split(' ');

                    int id = int.Parse(args[0]);
                    string name = args[1];

                    string fullTypeName = "Snow.Network.Packets.Play.Serverbound." + name;

                    Type packetType = Type.GetType(fullTypeName);

                    mappings.Add(id, packetType);
                }
            } catch(Exception ex)
            {
                Console.WriteLine("Failed to load serverbound mappings. \n" + ex);
            }
        }

        public static ServerboundPacket CreateNewPacket(byte[] packet)
        {
            try
            {
                int read = 0;
                int id = VarInt.FromByteArray(packet, out read);

                byte[] packetContent = new byte[packet.Length - read];
                Array.Copy(packet, read, packetContent, 0, packet.Length - read);

                if (!mappings.ContainsKey(id))
                {
                    return null;
                }

                ServerboundPacket serverboundPacket = (ServerboundPacket)Activator.CreateInstance(mappings[id]);

                PacketReader packetReader = new PacketReader(packetContent);
                serverboundPacket.Decode(packetReader);

                return serverboundPacket;
            } catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
