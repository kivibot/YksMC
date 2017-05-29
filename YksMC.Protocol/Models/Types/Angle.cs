using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Models.Types
{
    public class Angle
    {
        public byte Value { get; set; }

        public Angle(byte value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            Angle other = obj as Angle;
            if (other == null)
                return false;
            if (Value != other.Value)
                return false;
            return true;
        }
    }
}
