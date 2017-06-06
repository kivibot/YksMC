using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Bot.Chunk.MemoryOptimized
{
    public struct DataBlock
    {
        public byte LightData { get; set; }
        public byte BlockId { get; set; }
        public byte BlockMeta { get; set; }
    }
}
