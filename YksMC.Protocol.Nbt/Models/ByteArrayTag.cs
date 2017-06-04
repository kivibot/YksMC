using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class ByteArrayTag
    {
        public string Name { get; set; }
        public byte[] Value { get; set; }

        public ByteArrayTag(string name, byte[] value)
        {
            Name = name;
            Value = value;
        }
    }
}
