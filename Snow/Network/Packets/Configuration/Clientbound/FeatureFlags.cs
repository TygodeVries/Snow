using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Configuration.Clientbound
{
    public class FeatureFlags : ClientboundPacket
    {
        /* #TODO for full implementation
        // Make features be able to be toggleable
        */
        public FeatureFlags()
        {

        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WriteVarInt(0x08);

            packetWriter.WriteVarInt(1); // Number of flags enabled
            packetWriter.WriteString("minecraft:vanilla");
        }
    }
}
