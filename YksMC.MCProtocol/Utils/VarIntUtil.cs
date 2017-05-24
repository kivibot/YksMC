using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MCProtocol.Models.Types;

namespace YksMC.MCProtocol.Utils
{
    public static class VarIntUtil
    {

        public static byte[] EncodeVarInt(int value)
        {
            List<byte> data = new List<byte>();
            uint uValue = (uint)value;
            do
            {
                byte temp = (byte)(uValue & 0b01111111);
                uValue >>= 7;
                if (uValue != 0)
                    temp |= 0b10000000;
                data.Add(temp);
            } while (uValue != 0);
            return data.ToArray();
        }

        public static byte[] EncodeVarLong(long value)
        {
            List<byte> data = new List<byte>();
            ulong uValue = (ulong)value;
            do
            {
                byte temp = (byte)(uValue & 0b01111111);
                uValue >>= 7;
                if (uValue != 0)
                    temp |= 0b10000000;
                data.Add(temp);
            } while (uValue != 0);
            return data.ToArray();
        }

        public static int DecodeVarInt(byte[] data, int offset = 0)
        {
            int bytesRead = 0;
            int value = 0;
            byte cur;
            do
            {
                cur = data[offset + bytesRead];
                value |= (cur & 0b01111111) << 7 * bytesRead;
                bytesRead++;
            } while ((cur & 0b10000000) != 0);
            return value;
        }

        public static long DecodeVarLong(byte[] data, int offset = 0)
        {
            int bytesRead = 0;
            long value = 0;
            byte cur;
            do
            {
                cur = data[offset + bytesRead];
                value |= ((long)cur & 0b01111111) << 7 * bytesRead;
                bytesRead++;
            } while ((cur & 0b10000000) != 0);
            return value;
        }
    }
}
