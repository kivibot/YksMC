using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.BlockType
{
    public interface IBlockTypeRepository
    {
        IBlockType GetType(IBlockTypeIdentity identity);
    }
}
