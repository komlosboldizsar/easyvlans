using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace easyvlans.Helpers
{
    public static class BitFieldHelpers
    {

        public static bool GetBit(this byte[] array, int byteIndex, int bitIndex)
            => (((array[byteIndex] >> bitIndex) & 0x01) != 0);

        public static void SetBit(this ref byte @byte, int bitIndex, bool bitValue)
        {
            if (bitValue)
                @byte |= (byte)((0x01) << bitIndex);
            else
                @byte &= (byte)(((0x01) << bitIndex) ^ 0xFF);
        }

        public static void SetBit(this byte[] array, int byteIndex, int bitIndex, bool bitValue)
            => array[byteIndex].SetBit(bitIndex, bitValue);

    }
}
