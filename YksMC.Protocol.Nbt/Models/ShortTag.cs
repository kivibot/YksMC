using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class ShortTag : BaseTag
    {
        public short Value { get; set; }

        public ShortTag(string name, short value)
            : base(name)
        {
            Value = value;
        }
    }
}
