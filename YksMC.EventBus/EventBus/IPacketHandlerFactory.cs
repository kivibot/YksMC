using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Client.Injection;
using YksMC.Protocol.Packets;

namespace YksMC.Client.EventBus
{
    public interface IPacketHandlerFactory
    {
        IOwned<IEnumerable<IEventHandler<T>>> GetHandlers<T>();
    }
}
