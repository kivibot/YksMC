using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Biome;
using YksMC.MinecraftModel.Inventory;
using YksMC.MinecraftModel.ItemStack;

namespace YksMC.MinecraftModel.Block
{
    public class ContainerBlock : Block, IContainerBlock
    {
        private readonly IInventory _inventory;

        public ContainerBlock(string name, bool isSolid, bool isDiggable, double hardness, HarvestTier harvestTier, BlockMaterial material, bool isDangerous, bool isEmpty, IInventory inventory)
            : base(name, isSolid, isDiggable, hardness, harvestTier, material, isDangerous, isEmpty)
        {
            _inventory = inventory;
        }

        protected ContainerBlock(BlockTypeInfo blockTypeInfo, byte lightFromBlocks, byte lightFromSky, IBiome biome, byte dataValue, IInventory inventory)
            : base(blockTypeInfo, lightFromBlocks, lightFromSky, biome, dataValue)
        {
            _inventory = inventory;
        }

        public IInventory GetInventory()
        {
            return _inventory;
        }

        public IContainerBlock WithInventorySlot(int slot, IItemStack itemStack)
        {
            IInventory inventory = _inventory.ChangeSlot(slot, itemStack);
            return CreateContainerBlock(_biome, _lightFromBlocks, _lightFromSky, _dataValue, inventory);
        }

        protected override Block CreateBlock(IBiome biome, byte lightFromBlocks, byte lightFromSky, byte dataValue)
        {
            return CreateContainerBlock(biome, lightFromBlocks, lightFromSky, dataValue, _inventory);
        }

        protected virtual ContainerBlock CreateContainerBlock(IBiome biome, byte lightFromBlocks, byte lightFromSky, byte dataValue, IInventory inventory)
        {
            return new ContainerBlock(_typeInfo, lightFromBlocks, lightFromSky, biome, dataValue, inventory);
        }
    }
}
