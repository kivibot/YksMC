using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Utils;

namespace YksMC.Protocol
{
    public class MCPacketWriter : IMCPacketWriter
    {
        private readonly IMCConnection _connection;
        private readonly List<byte> _data;

        public MCPacketWriter(IMCConnection connection)
        {
            _connection = connection;
            _data = new List<byte>();
        }

        public void PutAngle(Angle angle)
        {
            PutByte(angle.Value);
        }

        public void PutBool(bool value)
        {
            PutByte(Convert.ToByte(value));
        }

        public void PutByte(byte value)
        {
            _data.Add(value);
        }

        public void PutBytes(byte[] data)
        {
            _data.AddRange(data);
        }

        public void PutChat(Chat value)
        {
            PutString(value.Value);
        }

        public void PutDouble(double value)
        {
            PutBytesReversed(BitConverter.GetBytes(value));
        }

        public void PutFloat(float value)
        {
            PutBytesReversed(BitConverter.GetBytes(value));
        }

        public void PutGuid(Guid value)
        {
            PutBytesReversed(value.ToByteArray());
        }

        public void PutInt(int value)
        {
            PutBytesReversed(BitConverter.GetBytes(value));
        }

        public void PutLong(long value)
        {
            PutBytesReversed(BitConverter.GetBytes(value));
        }

        public void PutPosition(Position value)
        {
            PutUnsignedLong((((ulong)value.X & 0x3FFFFFF) << 38) | (((ulong)value.Y & 0xFFF) << 26) | ((ulong)value.Z & 0x3FFFFFF));
        }

        public void PutShort(short value)
        {
            PutBytesReversed(BitConverter.GetBytes(value));
        }

        public void PutSignedByte(sbyte value)
        {
            PutByte((byte)value);
        }

        public void PutString(string value)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(value);
            PutVarInt(new VarInt(encoded.Length));
            PutBytes(encoded);
        }

        public void PutUnsignedInt(uint value)
        {
            PutBytesReversed(BitConverter.GetBytes(value));
        }

        public void PutUnsignedLong(ulong value)
        {
            PutBytesReversed(BitConverter.GetBytes(value));
        }

        public void PutUnsignedShort(ushort value)
        {
            PutBytesReversed(BitConverter.GetBytes(value));
        }

        public void PutVarInt(VarInt value)
        {
            PutBytes(VarIntUtil.EncodeVarInt(value.Value));
        }

        public void PutVarLong(VarLong value)
        {
            PutBytes(VarIntUtil.EncodeVarLong(value.Value));
        }

        public async Task SendPacketAsync(CancellationToken cancelToken = default(CancellationToken))
        {
            await _connection.SendPacketAsync(_data.ToArray(), cancelToken);

            _data.Clear();
        }

        private void PutBytesReversed(byte[] bytes)
        {
            Array.Reverse(bytes);
            PutBytes(bytes);
        }
    }
}
