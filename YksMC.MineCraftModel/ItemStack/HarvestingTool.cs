using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.BlockType;

namespace YksMC.MinecraftModel.ItemStack
{
    public class HarvestingTool : ItemStack, IHarvestingTool
    {
        private readonly HarvestTier _tier;
        private readonly double _breakingMultiplier;

        public HarvestingTool(string name, int maxDurability, HarvestTier harvestTier, double breakingMultiplier)
            : this(name, 1, 1, maxDurability, maxDurability, harvestTier, breakingMultiplier)
        {
        }

        protected HarvestingTool(string name, int size, int maxSize, int durability, int maxDurability, HarvestTier harvestTier, double breakingMultiplier)
            : base(name, size, maxSize, durability, maxDurability)
        {
            _tier = harvestTier;
            _breakingMultiplier = breakingMultiplier;
        }

        public bool CanHarvest(IBlockType blockType)
        {
            if(blockType.HarvestTier == HarvestTier.Hand)
            {
                return true;
            }
            //TODO: check material type/category (rock, wood, dirt)
            return blockType.HarvestTier < _tier;
        }

        public double GetBreakingSpeed(IBlockType blockType)
        {
            if (!CanHarvest(blockType))
            {
                return 0.2;
            }
            return (1.0 / 1.5) * _breakingMultiplier;
        }

        protected override IItemStack CreateItemStack(string name, int size, int maxSize, int durability, int maxDurability)
        {
            return new HarvestingTool(name, size, maxSize, durability, maxDurability, _tier, _breakingMultiplier);
        }
    }
}
