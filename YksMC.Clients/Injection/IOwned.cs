using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Clients.Injection
{
    public interface IOwned<T> : IDisposable
    {
        T Value { get; }
    }
}
