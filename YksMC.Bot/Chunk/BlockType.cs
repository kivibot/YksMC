using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Chunk;

namespace YksMC.Bot.Chunk
{
    public class BlockType : IBlockType
    {
        public int Id { get; set; }
        public int Data { get; set; }
        public string Name { get; set; }
        public double Hardness { get; set; }
        public int StackSize { get; set; }
        public bool IsDiggable { get; set; }
        public BoundingBoxType BoundingBox { get; set; }
        public bool IsTransparent { get; set; }
        public int EmitLight { get; set; }
        public int FilterLight { get; set; }
    }
}
