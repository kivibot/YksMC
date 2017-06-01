using Autofac.Features.OwnedInstances;
using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Clients.Injection
{
    public class AutofacOwnedWrapper<T> : IOwned<T>
    {
        private readonly Owned<T> _owned;

        public T Value => _owned.Value;

        public AutofacOwnedWrapper(Owned<T> owned)
        {
            _owned = owned; 
        }

        public void Dispose()
        {
            _owned.Dispose();
        }
    }
}
