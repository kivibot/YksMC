using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class LongTag
    {
        public string Name { get; set; }
        public long Value { get; set; }

        public LongTag(string name, long value)
        {
            Name = name;
            Value = value;
        }
    }
}
