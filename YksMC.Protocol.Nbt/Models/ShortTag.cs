using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class ShortTag
    {
        public string Name { get; set; }
        public short Value { get; set; }

        public ShortTag(string name, short value)
        {
            Name = name;
            Value = value;
        }
    }
}
