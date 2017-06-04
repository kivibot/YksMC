using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Nbt;

namespace YksMC.Protocol
{
    public interface IPacketReader : INbtDataReader
    {
        void SetPacket(byte[] packet);
        bool GetBool();
        sbyte GetSignedByte();
        ushort GetUnsignedShort();
        uint GetUnsignedInt();
        ulong GetUnsignedLong();
        string GetString();
        Chat GetChat();
        VarInt GetVarInt();
        VarLong GetVarLong();
        Position GetPosition();
        Angle GetAngle();
        Guid GetGuid();
        void ResetPosition();
        int GetRemainingBytes();
    }
}
