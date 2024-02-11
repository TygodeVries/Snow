using Snow.Formats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snow.Admin;
using System.Xml.Linq;
using System.Collections;
using System.Reflection;
namespace Snow.Network.Mappings
{
    public class PacketMappings
    {
        List<int> ids = new List<int>();
        List<Type> types = new List<Type>();

        public Type FromID(int id)
        {
            int index = ids.IndexOf(id);
            if (index == -1) return null;
            return types[index];
        }

        public int FromType(Type type)
        {
            int index = types.IndexOf(type);
            if (index == -1) return -1;
            return ids[index];
        }

        public int Size()
        {
            return ids.Count;
        }

        public PacketMappings(List<int> ids, List<Type> types)
        {
            this.ids = ids;
            this.types = types;
        }

        public PacketMappings()
        {

        }

        public void Add(int id, Type type)
        {
            ids.Add(id);
            types.Add(type);
        }
    }

    public class MappingsManager
    {
        static Dictionary<ConnectionState, PacketMappings> clientboundPackets;
        static Dictionary<ConnectionState, PacketMappings> serverboundPackets;

        public static void Load()
        {
            Log.Send("Loading mappings...");
            clientboundPackets = LoadPacketMappingsFromFile(@"Data/Packets/clientbound.packets", "Clientbound");
            serverboundPackets = LoadPacketMappingsFromFile(@"Data/Packets/serverbound.packets", "Serverbound");
        }

        private static Dictionary<ConnectionState, PacketMappings> LoadPacketMappingsFromFile(string path, string direction)
        {
            string[] lines = File.ReadAllLines(path);
            Dictionary<ConnectionState, PacketMappings> result = new Dictionary<ConnectionState, PacketMappings>();


            ConnectionState state = ConnectionState.HANDSHAKE;
            PacketMappings mappings = null;
            string folder = "unknown";

            for (int i = 0; i < lines.Length + 1; i++)
            {
                string line = "$ END";

                if (i < lines.Length)
                {
                     line = lines[i];
                }

                string[] args = line.Split(' ');

                if (args.Length < 2)
                    continue;

                if (args[0] == "$")
                {
                    if (mappings != null)
                    {
                        result.Add(state, mappings);
                    }

                    mappings = new PacketMappings();

                    if (args[1] == "END")
                        continue;

                    // Get state
                    state = (ConnectionState)Enum.Parse(typeof(ConnectionState), args[1].ToUpper());
                    folder = args[1];
                    continue;
                }

                if (args[0].StartsWith("//"))
                    continue;

                string fullTypeName = $"Snow.Network.Packets.{folder}.{direction}." + args[1];

                Type type = Type.GetType(fullTypeName);
                int id = Convert.ToInt32(args[0], 16);;

                mappings.Add(id, type);
            }

            return result;
        }

        public static int GetPacketIDOfPacket(ClientboundPacket packet)
        {
            // Example path 
            // Snow.Network.Packets.Configuration.Clientbound.FeatureFlags
            // 

            string path = packet.GetType().ToString();
            string[] folders = path.Split('.');

            string state = folders[folders.Length - 3].ToUpper();
            ConnectionState connectionState = (ConnectionState) Enum.Parse(typeof(ConnectionState), state);

            int id = clientboundPackets[connectionState].FromType(packet.GetType());
            return id;
        }

        public static ServerboundPacket CreateServerboundPacket(byte[] packet, Connection playerConnection)
        {
            try
            {
                int id = VarInt.FromByteArray(packet, out int read);

                byte[] packetContent = new byte[packet.Length - read];
                Array.Copy(packet, read, packetContent, 0, packet.Length - read);

                Type type = serverboundPackets[playerConnection.GetConnectionState()].FromID(id);

                if (type == null)
                {
                    return null;
                }

                ServerboundPacket serverboundPacket = (ServerboundPacket)Activator.CreateInstance(type);

                PacketReader packetReader = new PacketReader(packetContent);
                serverboundPacket.Decode(packetReader);

                return serverboundPacket;
            } catch(Exception ex)
            {
                return null;
            }
        }
    }
}
