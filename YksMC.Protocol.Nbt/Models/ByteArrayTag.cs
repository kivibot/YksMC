﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class ByteArrayTag : BaseTag
    {
        public byte[] Value { get; set; }

        public ByteArrayTag(string name, byte[] value)
            : base(name)
        {
            Value = value;
        }
    }
}
