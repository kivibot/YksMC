using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.MCProtocol.Models;
using YksMC.MCProtocol.Models.Types;

namespace YksMC.MCProtocol
{
    public interface IMCPacketReader
    {
        Task<bool> NextAsync(CancellationToken cancelToken = default(CancellationToken));

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
    }
}
