﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Models.Types
{
    public sealed class Angle
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

        public double GetRadians()
        {
            return Value / 256.0 * 2 * Math.PI;
        }
    }
}
