using Snow.Formats;
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

        public void WriteString(string s)
        {
            WriteVarInt(s.Length);
            
            WriteByteArray(Encoding.UTF8.GetBytes(s));
        }

        public void WriteByteArray(byte[] bytes)
        {
            for(int i = 0; i < bytes.Length; i++)
            {
                WriteByte(bytes[i]);
            }
        }

        public void WriteBool(bool b)
        {
            WriteByte(b ? (byte) 0 : (byte) 1);
        }

        public void WriteVarInt(int i)
        {
            byte[] bytes = VarInt.ToByteArray((uint)i);
            WriteByteArray(bytes);
        }
    }
}
