using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Nbt.Models
{
    public class CompoundTag
    {
        public string Name { get; set; }
        public List<BaseTag> Tags { get; set; }

        public CompoundTag(string name, List<BaseTag> tags)
        {
            Name = name;
            Tags = tags;
        }
    }
}
