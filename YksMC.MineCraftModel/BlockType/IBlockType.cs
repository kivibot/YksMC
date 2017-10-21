using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.BlockType
{
    public interface IBlockType
    {
        string Name { get; }
        bool IsSolid { get; }
        bool IsDiggable { get; }
        double Hardness { get; }
        HarvestTier HarvestTier { get; }
        BlockMaterial Material { get; }
    }
}
