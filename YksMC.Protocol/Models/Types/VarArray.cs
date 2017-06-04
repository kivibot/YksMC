using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YksMC.Protocol.Models.Types
{
    public sealed class VarArray<T>
    {
        public VarInt Count { get; set; }
        public T[] Values { get; set; }

        public override bool Equals(object obj)
        {
            VarArray<T> other = obj as VarArray<T>;
            if (other == null)
                return false;
            if (!object.Equals(Count, other.Count))
                return false;
            if (!Enumerable.SequenceEqual(Values, other.Values))
                return false;
            return true;
        }

        public static implicit operator VarArray<T>(T[] values)
        {
            return new VarArray<T>()
            {
                Count = values.Length,
                Values = values
            };
        }
    }
}
