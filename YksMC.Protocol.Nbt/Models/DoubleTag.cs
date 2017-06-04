using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class DoubleTag : BaseTag
    {
        public double Value { get; set; }

        public DoubleTag(double value)
        {
            Value = value;
        }
    }
}
