using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class DoubleTag
    {
        public string Name { get; set; }
        public double Value { get; set; }

        public DoubleTag(string name, double value)
        {
            Name = name;
            Value = value;
        }
    }
}
