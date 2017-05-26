using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Models.Types
{
    public class VarLong
    {
        public const int MaxLenght = 5;

        public long Value { get; set; }

        public VarLong(long value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            VarLong other = obj as VarLong;
            if (other == null)
                return false;
            return Value == other.Value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
