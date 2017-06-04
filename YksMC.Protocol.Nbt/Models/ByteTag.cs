using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class ByteTag : BaseTag
    {
        public byte Value { get; set; }

        public ByteTag(string name, byte value)
            : base(name)
        {
            Value = value;
        }
    }
}
