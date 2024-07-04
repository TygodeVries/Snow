using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Configuration.Clientbound
{
    public class FeatureFlagsPacket : ClientboundPacket
    {
        public string[] features;
        public FeatureFlagsPacket(string[] features)
        {
            this.features = features;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            packetWriter.WriteVarInt(features.Length);
            foreach(string feature in features)
            {
                packetWriter.WriteString(feature);
            }
        }
    }
}
