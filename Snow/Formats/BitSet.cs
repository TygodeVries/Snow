using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snow.Formats
{
    public class BitSet
    {
        public List<long> Out { get; set; }

        public BitSet()
        {
            Out = new List<long>();
        }

        public void Set(int index)
        {
            int arrayIndex = index / 64;
            int bitIndex = index % 64;
            while (Out.Count <= arrayIndex)
            {
                Out.Add(0);
            }
            Out[arrayIndex] |= (1L << bitIndex);
        }

        public bool AllZero()
        {
            foreach (var value in Out)
            {
                if (value != 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
