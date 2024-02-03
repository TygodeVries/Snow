﻿using Snow.Containers;
using Snow.Formats;
using Snow.Formats.Nbt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network
{
    internal class PacketWriter
    {
        public byte[] ToByteArray()
        {
            return bytes.ToArray();
        }

        List<byte> bytes = new List<byte>();

        public void WriteByte(byte b)
        {
            bytes.Add(b);
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

        public void WriteString(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            WriteVarInt(bytes.Length);
            WriteByteArray(bytes);
        }

        public void WriteByteArray(byte[] bytes)
        {
            for(int i = 0; i < bytes.Length; i++)
            {
                WriteByte(bytes[i]);
            }
        }

        public void WritePosition(Position pos)
        {
            WriteByteArray(pos.ToByteArray());
        }

        public void WriteSlotArray(Slot[] slots)
        {
            WriteVarInt(slots.Length);

            foreach(Slot slot in slots)
            {
                WriteSlot(slot);
            }
        }

        public void WriteSlot(Slot slot)
        {
            WriteBool(slot.Present);
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
            WriteByte(b ? (byte) 0 : (byte) 1);
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
    }
}
