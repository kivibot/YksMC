using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Clients.Injection;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Clients
{
    public interface IPacketHandlerFactory
    {
        IOwned<IEnumerable<IPacket>> GetHandlers(Type packetType);
    }
}
