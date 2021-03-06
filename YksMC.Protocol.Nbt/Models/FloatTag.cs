﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class FloatTag : BaseTag
    {
        public float Value { get; set; }

        public FloatTag(float value)
        {
            Value = value;
        }
    }
}
