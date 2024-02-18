using Snow.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network
{
    public class PacketReader
    {
        public byte[] data;

        public PacketReader(byte[] data)
        {
            this.data = data;

        }

        public int pointer = 0;

        public short ReadShort()
        {
            byte[] item = new byte[2];
            Array.Copy(data, pointer, item, 0, 2);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(item);
            }
            short value = BitConverter.ToInt16(item, 0);

            pointer += 2;
            return value;
        }

        public string ReadString()
        {
            int toRead = ReadVarInt();

            byte[] text = new byte[toRead];

            Array.Copy(data, pointer, text, 0, toRead);

            string value = Encoding.UTF8.GetString(text);
            return value;
        }

        public UUID ReadUUID()
        {
            byte[] bytes = new byte[16];
            Array.Copy(data, pointer, bytes, 0, 16);
            UUID uuid = new UUID(bytes);

            return uuid;
        }

        public int ReadVarInt()
        {
            int size = 20;
            if (data.Length - pointer < size)
                size = data.Length - pointer;

            byte[] bytes = new byte[size];
            Array.Copy(data, pointer, bytes, 0, size);
            int value = VarInt.FromByteArray(bytes, out int bytesRead);

            pointer += bytesRead;
            return value;
        }
        public double ReadDouble()
        {
            byte[] item = new byte[8];
            Array.Copy(data, pointer, item, 0, 8);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(item);
            }
            double value = BitConverter.ToDouble(item, 0);

            pointer += 8;
            return value;
        }

        public long ReadLong()
        {
            byte[] item = new byte[8];
            Array.Copy(data, pointer, item, 0, 8);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(item);
            }
            long value = BitConverter.ToInt64(item, 0);

            pointer += 8;
            return value;
        }

        public float ReadFloat()
        {
            byte[] item = new byte[4];
            Array.Copy(data, pointer, item, 0, 4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(item);
            }
            float value = BitConverter.ToInt32(item, 0);

            pointer += 4;
            return value;
        }

        public bool ReadBool()
        {
            bool value = data[pointer] == 0x01;
            pointer++;

            return value;
        }
    }
}
