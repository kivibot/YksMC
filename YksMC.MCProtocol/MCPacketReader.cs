using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.MCProtocol.Models;

namespace YksMC.MCProtocol
{
    public class MCPacketReader : IMCPacketReader
    {
        private readonly IMCPacketSource _packetSource;
        private byte[] _data;
        private int _index;

        public MCPacketReader(IMCPacketSource packetSource)
        {
            _packetSource = packetSource;
        }

        public Angle GetAngle()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public double GetDouble()
        {
            return BitConverter.Int64BitsToDouble(GetLong());
        }

        public float GetFloat()
        {
            return BitConverter.ToSingle(GetBytesReversed(4), 0);
        }

        public Guid GetGuid()
        {
            throw new NotImplementedException();
        }

        public int GetInt()
        {
            return (int)GetUnsignedInt();
        }

        public long GetLong()
        {
            return (long)GetUnsignedLong();
        }

        public Position GetPosition()
        {
            throw new NotImplementedException();
        }

        public short GetShort()
        {
            return (short)GetUnsignedShort();
        }

        public sbyte GetSignedByte()
        {
            return (sbyte)GetByte();
        }

        public string GetString()
        {
            throw new NotImplementedException();
        }

        public uint GetUnsignedInt()
        {
            return (uint)GetUnsignedShort() << 16 | GetUnsignedShort();
        }

        public ulong GetUnsignedLong()
        {
            return (ulong)GetUnsignedInt() << 32 | GetUnsignedInt();
        }

        public ushort GetUnsignedShort()
        {
            return (ushort)((int)GetByte() << 8 | GetByte());
        }

        public VarInt GetVarInt()
        {
            throw new NotImplementedException();
        }

        public VarLong GetVarLong()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> NextAsync(CancellationToken cancelToken = default(CancellationToken))
        {
            _data = await _packetSource.GetNextAsync(cancelToken);
            _index = 0;
            return _data != null;
        }
    }
}
