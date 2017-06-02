using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol
{
    public interface IPacketReader
    {
        void SetPacket(byte[] packet);

        bool GetBool();

        sbyte GetSignedByte();
        byte GetByte();

        short GetShort();
        ushort GetUnsignedShort();

        int GetInt();
        uint GetUnsignedInt();

        long GetLong();
        ulong GetUnsignedLong();

        float GetFloat();
        double GetDouble();

        string GetString();
        Chat GetChat();
        VarInt GetVarInt();
        VarLong GetVarLong();

        Position GetPosition();
        Angle GetAngle();

        Guid GetGuid();

        byte[] GetBytes(int count);

        ByteArray GetByteArray();

        void ResetPosition();
    }
}
