using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MCProtocol.Models
{
    public class VarLong
    {
        public long Value { get; set; }

        public VarLong(long value)
        {
            Value = value;
        }
    }
}
