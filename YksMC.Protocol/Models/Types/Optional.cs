using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;

namespace YksMC.Protocol.Models.Types
{
    public class Optional<T>
    {
        public bool HasValue { get; set; }
        [Conditional(nameof(HasValue), Condition.Is, true)]
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
