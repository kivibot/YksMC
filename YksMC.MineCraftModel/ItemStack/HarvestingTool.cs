using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.MinecraftModel.ItemStack
{
    public class HarvestingTool : ItemStack, IHarvestingTool
    {
        private readonly HarvestTier _tier;
        private readonly double _breakingMultiplier;
        private readonly BlockMaterial _effectiveMaterial;

        public HarvestingTool(string name, int maxDurability, HarvestTier harvestTier, double breakingMultiplier, BlockMaterial effectiveMaterial)
            : this(name, 1, 1, maxDurability, maxDurability, harvestTier, breakingMultiplier, effectiveMaterial)
        {
        }

        protected HarvestingTool(string name, int size, int maxSize, int durability, int maxDurability, HarvestTier harvestTier, double breakingMultiplier, BlockMaterial effectiveMaterial)
            : base(name, size, maxSize, durability, maxDurability)
        {
            _tier = harvestTier;
            _breakingMultiplier = breakingMultiplier;
            _effectiveMaterial = effectiveMaterial;
        }

        public bool CanHarvest(IBlock blockType)
        {
            if (blockType.HarvestTier == HarvestTier.Hand)
            {
                return true;
            }
            if (blockType.Material != _effectiveMaterial)
            {
                return false;
            }
            return blockType.HarvestTier <= _tier;
        }

        public double GetBreakingSpeed(IBlock blockType)
        {
            if (!CanHarvest(blockType) || blockType.Material != _effectiveMaterial)
            {
                return 0.2;
            }
            return _breakingMultiplier / 1.5;
        }

        protected override IItemStack CreateItemStack(string name, int size, int maxSize, int durability, int maxDurability)
        {
            return new HarvestingTool(name, size, maxSize, durability, maxDurability, _tier, _breakingMultiplier, _effectiveMaterial);
        }
    }
}
