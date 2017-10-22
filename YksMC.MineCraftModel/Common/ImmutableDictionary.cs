using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YksMC.MinecraftModel.Common
{
    public class ImmutableDictionary<TKey, TValue> : IImmutableDictionary<TKey, TValue>
    {
        private readonly IReadOnlyDictionary<TKey, TValue> _dictionary;

        public TValue this[TKey key] => _dictionary[key];
        public IEnumerable<TKey> Keys => _dictionary.Keys;
        public IEnumerable<TValue> Values => _dictionary.Values;
        public int Count => _dictionary.Count;

        public ImmutableDictionary()
            : this(new Dictionary<TKey, TValue>())
        {
        }

        public ImmutableDictionary(IReadOnlyDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public IImmutableDictionary<TKey, TValue> WithEntry(TKey key, TValue value)
        {
            Dictionary<TKey, TValue> dictionary = _dictionary.ToDictionary(e => e.Key, e => e.Value);
            dictionary[key] = value;
            return new ImmutableDictionary<TKey, TValue>(dictionary);
        }

        public IImmutableDictionary<TKey, TValue> WithoutEntry(TKey key)
        {
            Dictionary<TKey, TValue> dictionary = _dictionary.ToDictionary(e => e.Key, e => e.Value);
            dictionary.Remove(key);
            return new ImmutableDictionary<TKey, TValue>(dictionary);
        }
    }
}
