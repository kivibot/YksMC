using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Common
{
    public interface IImmutableDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        IImmutableDictionary<TKey, TValue> Replace(TKey key, TValue value);        
    }
}
