using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Models.Types
{
    public class Optional<T>
    {
        public bool HasValue { get; set; }
        public T Value { get; set; }

        public override bool Equals(object obj)
        {
            Optional<T> other = obj as Optional<T>;
            if (other == null)
                return false;
            if (HasValue != other.HasValue)
                return false;
            if (!object.Equals(Value, other.Value))
                return false;
            return true;
        }
    }
}
