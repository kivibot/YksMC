using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class CompoundTag : BaseTag
    {
        public List<BaseTag> Tags { get; set; }

        public CompoundTag(List<BaseTag> tags)
        {
            Tags = tags;
        }
    }
}
