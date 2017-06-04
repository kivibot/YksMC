using Autofac;
using Autofac.Features.OwnedInstances;
using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Client.Injection;

namespace YksMC.Client.Handler
{
    public class AutofacPacketHandlerFactory : IPacketHandlerFactory
    {
        private readonly IComponentContext _context;

        public AutofacPacketHandlerFactory(IComponentContext context)
        {
            _context = context;
        }

        public IOwned<IEnumerable<IPacketHandler<T>>> GetHandlers<T>()
        {
            return _context.Resolve<IOwned<IEnumerable<IPacketHandler<T>>>>();
        }
    }
}
