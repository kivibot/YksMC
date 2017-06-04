using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YksMC.Protocol.Models.Types
{
    public sealed class VarArray<TLen, TVal>
    {
        public TLen Count { get; set; }
        public TVal[] Values { get; set; }

        public override bool Equals(object obj)
        {
            VarArray<TLen, TVal> other = obj as VarArray<TLen, TVal>;
            if (other == null)
                return false;
            if (!object.Equals(Count, other.Count))
                return false;
            if (!Enumerable.SequenceEqual(Values, other.Values))
                return false;
            return true;
        }

        public static implicit operator VarArray<TLen, TVal>(TVal[] values)
        {
            return new VarArray<TLen, TVal>()
            {
                Count = (TLen)(object)values.Length,
                Values = values
            };
        }
    }
}
