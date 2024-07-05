using Snow.Formats.Nbt.Values;
using Snow.Formats.Nbt;
using Snow.Formats;
using Snow.Network;
using Snow.Network.Packets.Configuration.Clientbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Servers.Registries
{
    public class ChatTypeRegistry : Registry
    {
        public List<ChatType> chatTypes = new List<ChatType>();

        public override void SendPacketToConnection(Connection connection)
        {
            List<RegistryDataPacketEntry> entries = new List<RegistryDataPacketEntry>();

            foreach (ChatType entry in chatTypes)
            {
                NbtCompoundTag tag = new NbtCompoundTag();

                // Chat tag
                NbtCompoundTag chatTag = new NbtCompoundTag();
                chatTag.AddField("translation_key", new NbtStringTag(entry.chat.translationKey));

                if (entry.chat.style != null)
                {
                    chatTag.AddField("style", entry.chat.style);
                }

                chatTag.AddField("parameters", entry.chat.parameters);

                // Narration
                NbtCompoundTag narrationTag = new NbtCompoundTag();
                narrationTag.AddField("translation_key", new NbtStringTag(entry.narration.translationKey));

                if (entry.narration.style != null)
                {
                    narrationTag.AddField("style", entry.narration.style);
                }

                narrationTag.AddField("parameters", entry.narration.parameters);

                // Write to tag
                tag.AddField("chat", chatTag);
                tag.AddField("narration", narrationTag);

                entries.Add(new RegistryDataPacketEntry(entry.identifier, true, tag));
            }

            connection.SendPacket(new RegistryDataPacket(new Identifier("minecraft", "chat_type"), entries));
        }
    }

    public class ChatType
    {
        public Identifier identifier;
        public Decoration chat;
        public Decoration narration;

        public ChatType(Identifier identifier, Decoration chat, Decoration narration)
        {
            this.identifier = identifier;
            this.chat = chat;
            this.narration = narration;
        }
    }

    public class Decoration
    {
        public string translationKey;
        public NbtCompoundTag style = null;
        public NbtListTag parameters;

        public Decoration(string translationKey, NbtListTag parameters)
        {
            this.translationKey = translationKey;
            this.parameters = parameters;
        }
    }
}
