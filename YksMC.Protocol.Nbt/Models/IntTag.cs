using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class IntTag : BaseTag
    {
        public int Value { get; set; }

        public IntTag(string name, int value)
            : base(name)
        {
            Value = value;
        }
    }
}
