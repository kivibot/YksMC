using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MCProtocol.Models.Types
{
    public class Angle
    {
        public byte Value { get; set; }

        public Angle(byte value)
        {
            Value = value;
        }
    }
}
