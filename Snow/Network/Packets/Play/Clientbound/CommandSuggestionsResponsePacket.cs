using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    internal class CommandSuggestionsResponsePacket : ClientboundPacket
    {
        public int id;
        public int start;
        public int lenght;
        public string[] matches;

        public CommandSuggestionsResponsePacket(int id, int start, int lenght, string[] matches)
        {
            this.id = id;
            this.start = start;
            this.lenght = lenght;
            this.matches = matches;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteVarInt(id);

            packetWriter.WriteVarInt(start);
            packetWriter.WriteVarInt(lenght);
            packetWriter.WriteVarInt(matches.Length);

            foreach(string match in matches)
            {
                packetWriter.WriteString(match);
                packetWriter.WriteBool(true);
                packetWriter.WriteTextComponent(new Formats.TextComponent("Test"));
            }
        }
    }
}
