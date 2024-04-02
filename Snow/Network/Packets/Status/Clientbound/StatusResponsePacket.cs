using Snow.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Status.Clientbound
{
    public class StatusResponsePacket : ClientboundPacket
    {

        public string version;
        public string protocol;
        public string max;
        public string online;

        public string playerName = "test";
        public string playerId = "4566e69f-c907-48ee-8d71-d7ba5aa00d20";

        public string description;
        public string favicon;
        public string enforcesSecureChat;
        public string previewsChat;

        public StatusResponsePacket(string version, string protocol, string max, string online, string description, string favicon, string enforcesSecureChat, string previewsChat)
        {
            this.version = version;
            this.protocol = protocol;
            this.max = max;
            this.online = online;
            this.description = description;
            this.favicon = favicon;
            this.enforcesSecureChat = enforcesSecureChat;
            this.previewsChat = previewsChat;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);
            string json = @"{
        ""version"": {
            ""name"": ""{version}"",
            ""protocol"": {protocol}
        },
        ""players"": {
            ""max"": {max},
            ""online"": {online},
            ""sample"": [
                {
                    ""name"": ""{playerName}"",
                    ""id"": ""{playerId}""
                }
            ]
        },
        ""description"": {
            ""text"": ""{description}""
        },
        ""favicon"": ""{favicon}"",
        ""enforcesSecureChat"": {enforcesSecureChat},
        ""previewsChat"": {previewsChat}
    }";

            json = json.Replace("{version}", version)
                       .Replace("{protocol}", protocol.ToString())
                       .Replace("{max}", max.ToString())
                       .Replace("{online}", online.ToString())
                       .Replace("{playerName}", playerName)
                       .Replace("{playerId}", playerId)
                       .Replace("{description}", description)
                       .Replace("{favicon}", favicon)
                       .Replace("{enforcesSecureChat}", enforcesSecureChat.ToString().ToLower())
                       .Replace("{previewsChat}", previewsChat.ToString().ToLower());

            packetWriter.WriteString(json);
        }

    }
}
