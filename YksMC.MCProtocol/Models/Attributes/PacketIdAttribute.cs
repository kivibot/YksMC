using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MCProtocol.Models.Attributes
{
    public class PacketIdAttribute : Attribute
    {
        public int Value { get; set; }

        public PacketIdAttribute(int value)
        {
            Value = value;
        }
    }
}
