using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class DoubleTag : BaseTag
    {
        public double Value { get; set; }

        public DoubleTag(string name, double value)
            : base(name)
        {
            Value = value;
        }
    }
}
