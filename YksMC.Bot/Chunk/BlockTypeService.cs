using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Chunk;

namespace YksMC.Bot.Chunk
{
    public class BlockTypeService : IBlockTypeService
    {
        public IBlockType GetBlockType(int blockId, byte metadata)
        {
            //TODO: use real types
            return new BlockType() { Id = blockId, Data = metadata };
        }
    }
}
