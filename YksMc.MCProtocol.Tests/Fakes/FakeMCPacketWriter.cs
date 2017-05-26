using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Protocol;
using YksMC.Protocol.Models.Types;

namespace YksMc.Protocol.Tests.Fakes
{
    public class FakeMCPacketWriter : IMCPacketWriter
    {
        private readonly List<object> _data;

        public object[] Objects { get; private set; }

        public FakeMCPacketWriter()
        {
            _data = new List<object>();
        }

        public void PutAngle(Angle value)
        {
            _data.Add(value);
        }

        public void PutBool(bool value)
        {
            _data.Add(value);
        }

        public void PutByte(byte value)
        {
            _data.Add(value);
        }

        public void PutBytes(byte[] data)
        {
            _data.Add(data);
        }

        public void PutChat(Chat value)
        {
            _data.Add(value);
        }

        public void PutDouble(double value)
        {
            _data.Add(value);
        }

        public void PutFloat(float value)
        {
            _data.Add(value);
        }

        public void PutGuid(Guid value)
        {
            _data.Add(value);
        }

        public void PutInt(int value)
        {
            _data.Add(value);
        }

        public void PutLong(long value)
        {
            _data.Add(value);
        }

        public void PutPosition(Position value)
        {
            _data.Add(value);
        }

        public void PutShort(short value)
        {
            _data.Add(value);
        }

        public void PutSignedByte(sbyte value)
        {
            _data.Add(value);
        }

        public void PutString(string value)
        {
            _data.Add(value);
        }

        public void PutUnsignedInt(uint value)
        {
            _data.Add(value);
        }

        public void PutUnsignedLong(ulong value)
        {
            _data.Add(value);
        }

        public void PutUnsignedShort(ushort value)
        {
            _data.Add(value);
        }

        public void PutVarInt(VarInt value)
        {
            _data.Add(value);
        }

        public void PutVarLong(VarLong value)
        {
            _data.Add(value);
        }

        public Task SendPacketAsync(CancellationToken cancelToken = default(CancellationToken))
        {
            Objects = _data.ToArray();
            _data.Clear();

            return Task.FromResult(true);
        }
    }
}
