using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class StringTag
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public StringTag(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
