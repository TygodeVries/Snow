using Snow.Formats;
using Snow.Formats.Nbt;
using Snow.Items;
using Snow.Levels;
using Snow.Network.Mappings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network
{
    public class PacketWriter
    {
        public byte[] ToByteArray()
        {
            return bytes.ToArray();
        }

        List<byte> bytes = new List<byte>();

        public void OverwriteAllBytes(byte[] bytes)
        {
            this.bytes = bytes.ToList();
        }

        public void WriteByte(byte b)
        {
            bytes.Add(b);
        }

        public void WritePacketID(ClientboundPacket packet)
        {
            WriteVarInt(MappingsManager.GetPacketIDOfPacket(packet));
        }

        public void WriteByteArray(byte[] bytes)
        {
            this.bytes.AddRange(bytes);
        }

        public void WriteUUID(UUID uuid)
        {
            WriteByteArray(uuid.GetBytes());
        }

        public void WriteCompoundTag(NbtCompoundTag compoundTag)
        {
            WriteByteArray(compoundTag.ToByteArray());
        }

        public void WriteChat(Chat chat)
        {
            throw new NotImplementedException();
        }

        public void WriteArrayOfIdentifier(Identifier[] identifiers)
        {
            WriteVarInt(identifiers.Length);

            for (int i = 0; i < identifiers.Length; i++)
            {
                WriteIdentifier(identifiers[i]);
            }
        }

        public void WriteIdentifier(Identifier identifier)
        {
            WriteString(identifier.GetString());
        }
        
        public void WriteLongArray(long[] longs)
        {
            WriteVarInt(longs.Length);

            for(int i = 0; i < longs.Length; i++)
            {
                WriteLong(longs[i]);
            }
        }

        public void WriteTextComponent(TextComponent textComponent)
        {
            textComponent.Write(this);
        }

        public void WriteString(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            WriteVarInt(bytes.Length);
            WriteByteArray(bytes);
        }

        public void WritePosition(Position pos)
        {
            WriteByteArray(pos.ToByteArray());
        }

        public void WriteBitArray(BitArray bitArray)
        {
            int numLongs = (bitArray.Length + 63) / 64; // Calculate the number of longs required

            // Write the length in VarInt format
            WriteVarInt(numLongs);

            // Write the data
            long[] data = ConvertBitArrayToLongArray(bitArray);
            foreach (long value in data)
            {
                WriteLong(value);
            }
        }

        private long[] ConvertBitArrayToLongArray(BitArray bitArray)
        {
            int numLongs = (bitArray.Length + 63) / 64;
            long[] data = new long[numLongs];
            for (int i = 0; i < numLongs; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    int index = i * 64 + j;
                    if (index < bitArray.Length && bitArray[index])
                    {
                        data[i] |= (1L << j);
                    }
                }
            }
            return data;
        }

        public void WriteItemStack(ItemStack itemStack)
        {
            if(itemStack != null)
            {
                WriteBool(true);
                WriteVarInt(itemStack.GetItemType().GetNetworkId());
                WriteByte((byte) itemStack.GetAmount());
                WriteCompoundTag(itemStack.GetNbtData());
            }
            else
            {
                WriteBool(false);
            }
        }

        public void WriteLong(long Long)
        {
            byte[] bytes = BitConverter.GetBytes(Long);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            WriteByteArray(bytes);
        }

        public void WriteFloat(float Float)
        {
            byte[] bytes = BitConverter.GetBytes(Float);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            WriteByteArray(bytes);
        }

        public void WriteDouble(double Double)
        {
            byte[] bytes = BitConverter.GetBytes(Double);
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            WriteByteArray(bytes);
        }


        public void WriteBool(bool b)
        {
            if (b) WriteByte(0x01);
            else WriteByte(0x00);
        }

        public void WriteVarInt(int i)
        {
            byte[] bytes = VarInt.ToByteArray((uint) i);
            WriteByteArray(bytes);
        }

        public void WriteVarLong(long l)
        {
            byte[] bytes = VarLong.ToByteArray(l);
            WriteByteArray(bytes);
        }

        public void WriteInt(int i)
        {
            byte[] intBytes = BitConverter.GetBytes(i);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(intBytes); // Convert to little-endian if necessary
            }

            WriteByteArray(intBytes);
        }

        public void WriteShort(short s)
        {
            byte[] intBytes = BitConverter.GetBytes(s);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(intBytes); // Convert to little-endian if necessary
            }

            WriteByteArray(intBytes);
        }

        public void WriteAngle(byte b)
        {
            WriteByte(b);
        }

        public void Clear()
        {
            bytes.Clear();
        }
    }
}
