using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Client.Injection;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Client
{
    public interface IPacketHandlerFactory
    {
        IOwned<IEnumerable<IPacketHandler<T>>> GetHandlers<T>();
    }
}
