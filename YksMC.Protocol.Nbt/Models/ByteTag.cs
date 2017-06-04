using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class ByteTag
    {
        public string Name { get; set; }
        public byte Value { get; set; }

        public ByteTag(string name, byte value)
        {
            Name = name;
            Value = value;
        }
    }
}
