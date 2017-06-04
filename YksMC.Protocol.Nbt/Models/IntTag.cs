using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class IntTag
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public IntTag(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}
