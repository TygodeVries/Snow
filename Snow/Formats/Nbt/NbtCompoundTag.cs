using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats.Nbt
{
    internal class NbtCompoundTag : NbtTag
    {
        List<NbtCompoundTagEntry> entries = new List<NbtCompoundTagEntry>();

        public bool addFileHeader = false;

        public byte[] ToByteArray()
        {
            byte[] bytes = new byte[] { 0x0a };
            return bytes.Concat(Encode()).ToArray();
        }

        public override byte[] Encode()
        { 
            byte[] bytes = new byte[] { };
            if (addFileHeader)
            {
                bytes = bytes.Concat(EncodeStringLenght("file")).ToArray();
                bytes = bytes.Concat(Encoding.UTF8.GetBytes("file")).ToArray();
            }

            foreach (NbtCompoundTagEntry entry in entries)
            {
                bytes = bytes.Concat(new byte[] { entry.tag.type }).ToArray();       // Type ID
                bytes = bytes.Concat(EncodeStringLenght(entry.name)).ToArray();  // Length of name
                bytes = bytes.Concat(Encoding.UTF8.GetBytes(entry.name)).ToArray();  // name

                byte[] data = entry.tag.Encode();
                bytes = bytes.Concat(data).ToArray();                                // Data
            }

            bytes = bytes.Concat(new byte[] { 0x00 }).ToArray();                     // Tag end

            return bytes;
        }

        byte[] EncodeStringLenght(string s)
        {
            byte[] bytes = BitConverter.GetBytes((ushort)Encoding.UTF8.GetByteCount(s));

            // If the system is little-endian, reverse the byte order
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return bytes;
        }
    }
}
