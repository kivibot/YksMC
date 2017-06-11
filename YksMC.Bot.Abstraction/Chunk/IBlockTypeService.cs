using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Abstraction.Chunk
{
    public interface IBlockTypeService
    {
        IBlockType GetBlockType(int blockId, byte metadata);
    }
}
