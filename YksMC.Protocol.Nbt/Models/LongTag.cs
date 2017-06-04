using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class LongTag : BaseTag
    {
        public long Value { get; set; }

        public LongTag(long value)
        {
            Value = value;
        }
    }
}
