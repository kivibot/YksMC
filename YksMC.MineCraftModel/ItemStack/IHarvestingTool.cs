using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.MinecraftModel.ItemStack
{
    public interface IHarvestingTool : IItemStack
    {
        bool CanHarvest(IBlock blockType);
        double GetBreakingSpeed(IBlock blockType);
    }
}
