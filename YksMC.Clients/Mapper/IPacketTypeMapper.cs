using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Clients.Mapper
{
    public interface IPacketTypeMapper
    {
        void RegisterType<T>() where T : IPacket;
        Type GetPacketType(ConnectionState connectionState, BoundTo boundTo, int id);
    }
}
