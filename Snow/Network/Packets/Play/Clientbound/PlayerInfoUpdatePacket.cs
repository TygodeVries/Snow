using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class PlayerInfoUpdatePacket : ClientboundPacket
    {
        byte? action = null;
        UUID uuid;

        private string name;

        private bool listed;

        public PlayerInfoUpdatePacket(UUID uuid)
        {
            this.uuid = uuid;
        }



        public override void Create(PacketWriter packetWriter)
        {
            if(action == null)
            {
                Log.Err("No payload loaded onto PlayerInfoUpdatePacket");
                return;
            }

            packetWriter.WritePacketID(this);
            packetWriter.WriteByte(action.Value);
            packetWriter.WriteVarInt(1);

            packetWriter.WriteUUID(uuid);

            if(action == 0x01)
            {
                packetWriter.WriteString(name);
                packetWriter.WriteVarInt(0);
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

        private void ConstructUpdateGameModePacket(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(1);
        }

        private void ConstructInitializeChatPacket(PacketWriter packetWriter)
        {
            packetWriter.WriteBool(false);
        }

        public void SetAddPlayerPayload(string name)
        {
            action = 0x01;
            this.name = name;
        }
    }
}
