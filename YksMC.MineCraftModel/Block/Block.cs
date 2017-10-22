using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using YksMC.MinecraftModel.Biome;

namespace YksMC.MinecraftModel.Block
{
    public class Block : IBlock
    {
        // Ram usage optimization
        protected readonly BlockTypeInfo _typeInfo;
        protected readonly byte _lightFromBlocks;
        protected readonly byte _lightFromSky;
        protected readonly IBiome _biome;
        protected readonly byte _dataValue;

        public string Name => _typeInfo.Name;
        public byte LightFromBlocks => _lightFromBlocks;
        public byte LightFromSky => _lightFromSky;
        public IBiome Biome => _biome;
        public bool IsSolid => _typeInfo.IsSolid;
        public bool IsDiggable => _typeInfo.IsDiggable;
        public double Hardness => _typeInfo.Hardness;
        public HarvestTier HarvestTier => _typeInfo.HarvestTier;
        public BlockMaterial Material => _typeInfo.Material;
        public bool IsDangerous => _typeInfo.IsDangerous;
        public bool IsEmpty => _typeInfo.IsEmpty;

        public Block(string name, bool isSolid, bool isDiggable, double hardness, HarvestTier harvestTier,
            BlockMaterial material, bool isDangerous, bool isEmpty)
        {
            _typeInfo = new BlockTypeInfo(name, isSolid, isDiggable, hardness, harvestTier, material, isDangerous, isEmpty);
            _lightFromBlocks = 0;
            _lightFromSky = 0;
            _biome = null; //TODO?
            _dataValue = 0;
        }

        protected Block(BlockTypeInfo blockTypeInfo, byte lightFromBlocks, byte lightFromSky, IBiome biome, byte dataValue)
        {
            _typeInfo = blockTypeInfo;
            _lightFromBlocks = lightFromBlocks;
            _lightFromSky = lightFromSky;
            _biome = biome;
            _dataValue = dataValue;
        }

        public IBlock WithBiome(IBiome biome)
        {
            return CreateBlock(biome, _lightFromBlocks, _lightFromSky, _dataValue);
        }

        public IBlock WithLightFromBlocks(byte lightLevel)
        {
            return CreateBlock(_biome, lightLevel, _lightFromSky, _dataValue);
        }

        public IBlock WithLightFromSky(byte lightLevel)
        {
            return CreateBlock(_biome, _lightFromBlocks, lightLevel, _dataValue);
        }

        protected virtual Block CreateBlock(IBiome biome, byte lightFromBlocks, byte lightFromSky, byte dataValue)
        {
            return new Block(_typeInfo, lightFromBlocks, lightFromSky, biome, dataValue);
        }

        public IBlock WithDataValue(byte metadata)
        {
            return CreateBlock(_biome, _lightFromBlocks, _lightFromSky, metadata);
        }
    }
}
