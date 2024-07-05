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
    public class DamageTypeRegistry : Registry
    {
        public List<DamageType> damageTypes = new List<DamageType>();

        public override void SendPacketToConnection(Connection connection)
        {
            List<RegistryDataPacketEntry> entries = new List<RegistryDataPacketEntry>();

            foreach (DamageType entry in damageTypes)
            {
                NbtCompoundTag tag = new NbtCompoundTag();
                tag.AddField("message_id", new NbtStringTag(entry.messageId));
                tag.AddField("scaling", new NbtStringTag(entry.scaling));
                tag.AddField("exhaustion", new NbtFloatTag(entry.exhaustion));

                entries.Add(new RegistryDataPacketEntry(entry.identifier, true, tag));
            }

            connection.SendPacket(new RegistryDataPacket(new Identifier("minecraft", "damage_type"), entries));
        }
    }

    public class DamageType
    {
        public Identifier identifier;

        public string messageId;
        public string scaling;
        public float exhaustion;

        public DamageType(Identifier identifier, string messageId, string scaling, float exhaustion)
        {
            this.identifier = identifier;
            this.messageId = messageId;
            this.scaling = scaling;
            this.exhaustion = exhaustion;
        }
    }
}
