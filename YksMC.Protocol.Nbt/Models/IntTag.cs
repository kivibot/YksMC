using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class IntTag : BaseTag
    {
        public int Value { get; set; }

        public IntTag(int value)
        {
            Value = value;
        }
    }
}
