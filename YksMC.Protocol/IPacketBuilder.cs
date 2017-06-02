using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol
{
    public interface IPacketBuilder
    {
        byte[] TakePacket();

        void PutBool(bool value);

        void PutSignedByte(sbyte value);
        void PutByte(byte value);

        void PutShort(short value);
        void PutUnsignedShort(ushort value);

        void PutInt(int value);
        void PutUnsignedInt(uint value);

        void PutLong(long value);
        void PutUnsignedLong(ulong value);

        void PutFloat(float value);
        void PutDouble(double value);

        void PutString(string value);
        void PutChat(Chat value);
        void PutVarInt(VarInt value);
        void PutVarLong(VarLong value);

        void PutPosition(Position value);
        void PutAngle(Angle value);

        void PutGuid(Guid value);

        void PutBytes(byte[] data);

        void PutByteArray(ByteArray value);
    }
}
