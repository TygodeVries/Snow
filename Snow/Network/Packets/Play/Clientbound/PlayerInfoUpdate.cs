using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    internal class PlayerInfoUpdate : ClientboundPacket
    {
        byte action;
        UUID uuid;

        public PlayerInfoUpdate(byte action, UUID uuid)
        {
            this.action = action;
            this.uuid = uuid;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x3C);
            packetWriter.WriteByte(action);
            packetWriter.WriteVarInt(1);

            packetWriter.WriteUUID(uuid);

            switch (action)
            {
                case 0x01:
                    ConstructAddPlayerPacket(packetWriter);
                    break;
                case 0x02:
                    ConstructInitializeChatPacket(packetWriter);
                    break;
                case 0x04:
                    ConstructUpdateGameModePacket(packetWriter);
                    break;
                case 0x08:
                    ConstructUpdateListedPacket(packetWriter);
                    break;
                case 0x10:
                    ConstructUpdateLatencyPacket(packetWriter);
                    break;
                case 0x20:
                    ConstructUpdateDisplayNamePacket(packetWriter);
                    break;
            }
        }

        private void ConstructUpdateDisplayNamePacket(PacketWriter packetWriter)
        {
            packetWriter.WriteBool(false);
        }

        private void ConstructUpdateLatencyPacket(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(10);
        }

        private void ConstructUpdateListedPacket(PacketWriter packetWriter)
        {
            packetWriter.WriteBool(true);
        }

        private void ConstructUpdateGameModePacket(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(1);
        }

        private void ConstructInitializeChatPacket(PacketWriter packetWriter)
        {
            packetWriter.WriteBool(false);
        }

        private void ConstructAddPlayerPacket(PacketWriter packetWriter)
        {
            packetWriter.WriteString("TheSheepDev");
            packetWriter.WriteVarInt(0);
        }
    }
}
