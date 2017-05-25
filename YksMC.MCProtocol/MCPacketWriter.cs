using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YksMC.MCProtocol.Models.Types;

namespace YksMC.MCProtocol
{
    public class MCPacketWriter : IMCPacketWriter
    {
        public MCPacketWriter(IMCPacketSink packetSink)
        {

        }

        public void PutAngle(Angle value)
        {
            throw new NotImplementedException();
        }

        public void PutBool(bool value)
        {
            throw new NotImplementedException();
        }

        public void PutByte(byte value)
        {
            throw new NotImplementedException();
        }

        public void PutBytes(byte[] data)
        {
            throw new NotImplementedException();
        }

        public void PutChat(Chat value)
        {
            throw new NotImplementedException();
        }

        public void PutDouble(double value)
        {
            throw new NotImplementedException();
        }

        public void PutFloat(float value)
        {
            throw new NotImplementedException();
        }

        public void PutGuid(Guid value)
        {
            throw new NotImplementedException();
        }

        public void PutInt(int value)
        {
            throw new NotImplementedException();
        }

        public void PutLong(long value)
        {
            throw new NotImplementedException();
        }

        public void PutPosition(Position value)
        {
            throw new NotImplementedException();
        }

        public void PutShort(short value)
        {
            throw new NotImplementedException();
        }

        public void PutSignedByte(sbyte value)
        {
            throw new NotImplementedException();
        }

        public void PutString(string value)
        {
            throw new NotImplementedException();
        }

        public void PutUnsignedInt(uint value)
        {
            throw new NotImplementedException();
        }

        public void PutUnsignedLong(ulong value)
        {
            throw new NotImplementedException();
        }

        public void PutUnsignedShort(ushort value)
        {
            throw new NotImplementedException();
        }

        public void PutVarInt(VarInt value)
        {
            throw new NotImplementedException();
        }

        public void PutVarLong(VarLong value)
        {
            throw new NotImplementedException();
        }

        public Task SendPacketAsync()
        {
            throw new NotImplementedException();
        }
    }
}
