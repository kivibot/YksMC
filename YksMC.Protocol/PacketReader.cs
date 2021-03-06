﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol
{
    public class PacketReader : IPacketReader
    {
        private byte[] _data;
        private int _index;

        public PacketReader()
        {
        }

        public Angle GetAngle()
        {
            return new Angle(GetByte());
        }

        public bool GetBool()
        {
            return Convert.ToBoolean(GetByte());
        }

        public byte GetByte()
        {
            return _data[_index++];
        }

        public byte[] GetBytes(int count)
        {
            byte[] values = new byte[count];
            Array.Copy(_data, _index, values, 0, count);
            _index += count;
            return values;
        }

        private byte[] GetBytesReversed(int count)
        {
            byte[] values = new byte[count];
            for (int i = 0; i < count; i++)
                values[i] = _data[_index + count - i - 1];
            _index += count;
            return values;
        }

        public Chat GetChat()
        {
            return new Chat(GetString());
        }

        public double GetDouble()
        {
            return BitConverter.ToDouble(GetBytesReversed(8), 0);
        }

        public float GetFloat()
        {
            return BitConverter.ToSingle(GetBytesReversed(4), 0);
        }

        public Guid GetGuid()
        {
            return new Guid(GetBytesReversed(16));
        }

        public int GetInt()
        {
            return BitConverter.ToInt32(GetBytesReversed(4), 0);
        }

        public long GetLong()
        {
            return BitConverter.ToInt64(GetBytesReversed(8), 0);
        }

        public Position GetPosition()
        {
            ulong val = GetUnsignedLong();
            return new Position(val);
        }

        public short GetShort()
        {
            return BitConverter.ToInt16(GetBytesReversed(2), 0);
        }

        public sbyte GetSignedByte()
        {
            return (sbyte)GetByte();
        }

        public string GetString()
        {
            VarInt length = GetVarInt();
            byte[] bytes = GetBytes(length.Value);
            return Encoding.UTF8.GetString(bytes);
        }

        public uint GetUnsignedInt()
        {
            return BitConverter.ToUInt32(GetBytesReversed(4), 0);
        }

        public ulong GetUnsignedLong()
        {
            return BitConverter.ToUInt64(GetBytesReversed(8), 0);
        }

        public ushort GetUnsignedShort()
        {
            return BitConverter.ToUInt16(GetBytesReversed(2), 0);
        }

        public VarInt GetVarInt()
        {
            int bytesRead = 0;
            int value = 0;
            byte cur;
            do
            {
                cur = GetByte();
                value |= (cur & 0b01111111) << 7 * bytesRead;
                bytesRead++;
            } while ((cur & 0b10000000) != 0);
            return new VarInt(value);
        }

        public VarLong GetVarLong()
        {
            int bytesRead = 0;
            long value = 0;
            byte cur;
            do
            {
                cur = GetByte();
                value |= ((long)cur & 0b01111111) << 7 * bytesRead;
                bytesRead++;
            } while ((cur & 0b10000000) != 0);
            return new VarLong(value);
        }

        public void ResetPosition()
        {
            _index = 0;
        }

        public void SetPacket(byte[] packet)
        {
            _data = packet;
            ResetPosition();
        }

        public int GetRemainingBytes()
        {
            return _data.Length - _index;
        }
    }
}
