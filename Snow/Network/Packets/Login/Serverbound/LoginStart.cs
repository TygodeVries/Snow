using Snow.Admin;
using Snow.Formats;
using Snow.Level;
using Snow.Level.Entities;
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
            Player entityPlayer = new Player(connection);

            LevelSpace levelSpace = connection.minecraftServer.GetLevelSpace();
            levelSpace.SpawnEntity(entityPlayer); // Spawn entity into world

            entityPlayer.username = username;

            Log.Send($"{username} joined the server!");

            connection.SendConnectionPackets(entityPlayer, username);

            entityPlayer.GetInventory().SetItem(36, new ItemStack(1, 0x01));
            entityPlayer.SpawnClient();

            connection.SetConnectionState(ConnectionState.PLAY);
        }

        string username;
        UUID uuid; // unused

        public override void Decode(PacketReader packetReader)
        {
            username = packetReader.ReadString();
            uuid = packetReader.ReadUUID();
        }
    }
}
