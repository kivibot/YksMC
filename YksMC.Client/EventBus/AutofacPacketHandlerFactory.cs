using Autofac;
using Autofac.Features.OwnedInstances;
using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Client.Injection;

namespace YksMC.Client.EventBus
{
    public class AutofacPacketHandlerFactory : IPacketHandlerFactory
    {
        private readonly IComponentContext _context;

        public AutofacPacketHandlerFactory(IComponentContext context)
        {
            _context = context;
        }

        public IOwned<IEnumerable<IEventHandler<T>>> GetHandlers<T>()
        {
            return _context.Resolve<IOwned<IEnumerable<IEventHandler<T>>>>();
        }
    }
}
