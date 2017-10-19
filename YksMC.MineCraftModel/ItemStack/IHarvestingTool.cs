using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.BlockType;

namespace YksMC.MinecraftModel.ItemStack
{
    public interface IHarvestingTool : IItemStack
    {
        bool CanHarvest(IBlockType blockType);
        double GetBreakingSpeed(IBlockType blockType);
    }
}
