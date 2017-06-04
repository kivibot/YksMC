using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class FloatTag
    {
        public string Name { get; set; }
        public float Value { get; set; }

        public FloatTag(string name, float value)
        {
            Name = name;
            Value = value;
        }
    }
}
