using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Models;

namespace YksMC.Bot.Chunk.MemoryOptimized
{
    public class Block : IBlock
    {
        public IBlockType Type { get; }
        public byte BlockLight { get; }
        public byte SkyLight { get; }

        public Block(IBlockType type, byte blockLight, byte skyLight)
        {
            Type = type;
            BlockLight = blockLight;
            SkyLight = skyLight;
        }
    }
}
