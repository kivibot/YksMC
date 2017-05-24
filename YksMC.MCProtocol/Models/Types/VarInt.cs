using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MCProtocol.Models.Types
{
    public class VarInt
    {
        public const int MaxLength = 5;

        public int Value { get; set; }

        public VarInt(int value)
        {
            Value = value;
        }
    }
}
