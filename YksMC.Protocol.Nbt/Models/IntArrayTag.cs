using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class IntArrayTag : BaseTag
    {
        public int[] Values { get; set; }

        public IntArrayTag(int[] values) 
        {
            Values = values;
        }
    }
}
