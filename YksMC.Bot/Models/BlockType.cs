using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Bot.Models
{
    public class BlockType
    {
        public int BlockId { get; set; }
        public string Name { get; set; }
        public double Hardness { get; set; }
        public int StackSize { get; set; }
        public bool Diggable { get; set; }
        public BoundingBoxType BoundingBox { get; set; }
    }
}
