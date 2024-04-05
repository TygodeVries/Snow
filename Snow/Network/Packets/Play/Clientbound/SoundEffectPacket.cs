using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network.Packets.Play.Clientbound
{
    public class SoundEffectPacket : ClientboundPacket
    {

        Identifier name;
        SoundSource source;
        float volume; 
        float pitch;
        long seed;

        public SoundEffectPacket(Identifier name, SoundSource source, float volume, float pitch, long seed)
        {
            this.name = name;
            this.source = source;
            this.volume = volume;
            this.pitch = pitch;
            this.seed = seed;
        }

        public override void Create(PacketWriter packetWriter)
        {
            packetWriter.WritePacketID(this);

            // name
            packetWriter.WriteVarInt(0);
            packetWriter.WriteIdentifier(name);
            
            // has fixed range
            packetWriter.WriteBool(false);

            // sound catergory
            packetWriter.WriteVarInt((int) source);

            // Effect Posistion
            packetWriter.WriteInt(0);
            packetWriter.WriteInt(0);
            packetWriter.WriteInt(0);

            
            packetWriter.WriteFloat(volume);
            packetWriter.WriteFloat(pitch);
            packetWriter.WriteLong(seed);
        }
    }

    public enum SoundSource
    {
        MASTER,
        MUSIC,
        RECORDS,
        WEATHER,
        BLOCKS,
        HOSTILE,
        NEUTRAL,
        PLAYERS,
        AMBIENT,
        VOICE
    }
}
