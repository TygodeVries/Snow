using Snow.Formats.Nbt;
using Snow.Formats.Nbt.Values;
using Snow.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats
{
    public class TextComponent
    {
        string text;

        public TextComponent(string text)
        {
            this.text = text;
        }

        public void Write(PacketWriter packetWriter)
        {
            NbtCompoundTag tag = new NbtCompoundTag();

            tag.AddField("text", new NbtStringTag(text));

            packetWriter.WriteCompoundTag(tag);
        }
    }
}
