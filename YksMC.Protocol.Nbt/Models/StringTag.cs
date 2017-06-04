using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class StringTag : BaseTag
    {
        public string Value { get; set; }

        public StringTag(string value)
        {
            Value = value;
        }
    }
}
