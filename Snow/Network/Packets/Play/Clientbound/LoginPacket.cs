using Snow.Entities;
using Snow.Formats;
using Snow.Servers.Registries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class LoginPacket : ClientboundPacket
    {
        public int playerId;
        public bool isHardcore;

        public List<Identifier> dimensions;
        public int viewDistance;
        public int simulationDistance;
        public bool reducedDebugInfo;
        public bool enableRespawnScreen;
        public bool doLimitedCrafting;
        public int currentDimensionId;
        public Identifier currentDimensionName;
        public byte[] hashedSeed;
        public byte gamemode;
        public byte previousGamemode;
        public bool isDebugWorld;
        public bool isFlatWorld;
        public int portalCooldown;
        public bool enforcesSecureChat;

        public LoginPacket(int playerId, bool isHardcore, List<Identifier> dimensions, int viewDistance, int simulationDistance, bool reducedDebugInfo, bool enableRespawnScreen, bool doLimitedCrafting, int currentDimensionId, Identifier currentDimensionName, byte[] hashedSeed, byte gamemode, byte previousGamemode, bool isDebugWorld, bool isFlatWorld, int portalCooldown, bool enforcesSecureChat)
        {
            this.playerId = playerId;
            this.isHardcore = isHardcore;
            this.dimensions = dimensions;
            this.viewDistance = viewDistance;
            this.simulationDistance = simulationDistance;
            this.reducedDebugInfo = reducedDebugInfo;
            this.enableRespawnScreen = enableRespawnScreen;
            this.doLimitedCrafting = doLimitedCrafting;
            this.currentDimensionId = currentDimensionId;
            this.currentDimensionName = currentDimensionName;
            this.hashedSeed = hashedSeed;
            this.gamemode = gamemode;
            this.previousGamemode = previousGamemode;
            this.isDebugWorld = isDebugWorld;
            this.isFlatWorld = isFlatWorld;
            this.portalCooldown = portalCooldown;
            this.enforcesSecureChat = enforcesSecureChat;
        }

        public override void Create(PacketWriter packetWriter)
        {
            if(hashedSeed.Length != 8)
            {
                Log.Err("Hashed seed must be 8 bytes long.");
                return;
            }

            packetWriter.WritePacketID(this);

            packetWriter.WriteInt(playerId); 
            packetWriter.WriteBool(isHardcore); 
            
            packetWriter.WriteVarInt(dimensions.Count); // Dimension Count
            foreach (Identifier dimension in dimensions)
            {
                packetWriter.WriteIdentifier(dimension);
            }

            packetWriter.WriteVarInt(1); // Max player count, Ignored

            packetWriter.WriteVarInt(viewDistance);
            packetWriter.WriteVarInt(simulationDistance);

            packetWriter.WriteBool(reducedDebugInfo);
            packetWriter.WriteBool(enableRespawnScreen);
            packetWriter.WriteBool(doLimitedCrafting);

            packetWriter.WriteVarInt(currentDimensionId);
            packetWriter.WriteIdentifier(currentDimensionName);

            packetWriter.WriteByteArray(hashedSeed);
          
            packetWriter.WriteByte(gamemode);
            packetWriter.WriteByte(previousGamemode);

            packetWriter.WriteBool(isDebugWorld);
            packetWriter.WriteBool(isFlatWorld);

            packetWriter.WriteBool(false); // Death location, will do later
            
            packetWriter.WriteVarInt(portalCooldown);
            packetWriter.WriteBool(enforcesSecureChat);

        }
    }
}
