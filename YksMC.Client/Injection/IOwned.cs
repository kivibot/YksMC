using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Client.Injection
{
    public interface IOwned<T> : IDisposable
    {
        T Value { get; }
    }
}
