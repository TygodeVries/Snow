using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Network
{
    internal class PacketReader
    {
        public byte[] data;

        public PacketReader(byte[] data)
        {
            this.data = data;

        }

        public int pointer = 0;

        public short ReadShort()
        {
            short value = BitConverter.ToInt16(data, pointer);
            pointer += 2;
            return value;
        }
    }
}
