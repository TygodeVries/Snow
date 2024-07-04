using Snow.Entities;
using Snow.Events;
using Snow.Formats;
using Snow.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Login.Serverbound
{
    internal class LoginStartPacket : ServerboundPacket
    {
        string username;
        UUID uuid;

        public override void Decode(PacketReader packetReader)
        {
            username = packetReader.ReadString();
            uuid = packetReader.ReadUUID();
        }

        public override void Use(Connection connection)
        {
            // Get the server
            Server server = connection.GetServer();

            // Create the player object
            Player player = new Player(connection);
            player.SetName(this.username);

            // Spawn in the entity
            server.GetWorldManager().GetDefaultWorld().SpawnEntity(player);
            
            // Upgrade connection.
            connection.Connect(server, player);
            connection.SetConnectionState(ConnectionState.PLAY);

            player.SyncClient();
            Log.Send($"{username} joined the server!");
        }
    }
}
