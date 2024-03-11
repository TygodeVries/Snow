using Snow.Entities;
using Snow.Events.Args;
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
        public override void Use(Connection connection)
        {
            Player player = new Player(connection);

            Server server = connection.GetServer();
            server.GetWorld().SpawnEntity(player);

            player.SetName(this.username);

            Log.Send($"{username} joined the server!");

            connection.Connect(server, player);
            player.SpawnClient();

            connection.SetConnectionState(ConnectionState.PLAY);

            connection.GetServer().GetEventManager().ExecutePlayerJoin(new PlayerJoinEventArgs(player));
        }

        string username;
        UUID uuid;

        public override void Decode(PacketReader packetReader)
        {
            username = packetReader.ReadString();
            uuid = packetReader.ReadUUID();
        }
    }
}
