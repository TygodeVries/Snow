using Snow.Entities;
using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Login.Serverbound
{
    internal class LoginStart : ServerboundPacket
    {
        public override void Use(Connection connection)
        {
            Player player = new Player(connection);

            Lobby lobby = connection.GetLobby();
            lobby.SpawnEntity(player);

            player.SetName(this.username);

            Log.Send($"{username} joined the server!");

            connection.Connect(lobby, player);
            player.SpawnClient();

            connection.SetConnectionState(ConnectionState.PLAY);
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
