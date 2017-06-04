using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class ListTag : BaseTag
    {
        public IList<BaseTag> Values { get; set; }

        public ListTag(IList<BaseTag> values) 
        {
            Values = values;
        }
    }
}
