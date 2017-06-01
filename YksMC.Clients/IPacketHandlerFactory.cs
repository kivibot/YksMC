using Autofac.Features.OwnedInstances;
using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Clients
{
    public interface IPacketHandlerFactory
    {
        Owned<IEnumerable<IPacket>> GetHandlers(Type packetType);
    }
}
