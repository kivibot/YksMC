using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Models.Types
{
    public class VarInt
    {
        public const int MaxLength = 5;

        public int Value { get; set; }

        public VarInt(int value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            VarInt other = obj as VarInt;
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
