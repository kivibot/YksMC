using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Common
{
    public interface IImmutableDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        IImmutableDictionary<TKey, TValue> WithEntry(TKey key, TValue value);
        IImmutableDictionary<TKey, TValue> WithoutEntry(TKey key);
    }
}
