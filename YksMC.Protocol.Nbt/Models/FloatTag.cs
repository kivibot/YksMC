using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class FloatTag : BaseTag
    {
        public float Value { get; set; }

        public FloatTag(string name, float value)
            : base(name)
        {
            Value = value;
        }
    }
}
