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
            return BitConverter.ToDouble(GetBytesReversed(8), 0);
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
            return BitConverter.ToInt32(GetBytesReversed(4), 0);
        }

        public long GetLong()
        {
            return BitConverter.ToInt64(GetBytesReversed(8), 0);
        }

        public Position GetPosition()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
