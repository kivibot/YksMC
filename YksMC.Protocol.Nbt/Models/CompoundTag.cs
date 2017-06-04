using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class CompoundTag : BaseTag
    {
        public List<BaseTag> Tags { get; set; }

        public CompoundTag(string name, List<BaseTag> tags)
            : base(name)
        {
            Tags = tags;
        }
    }
}
