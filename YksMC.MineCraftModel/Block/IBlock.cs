using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Biome;

namespace YksMC.MinecraftModel.Block
{
    public interface IBlock
    {
        string Name { get; }
        byte LightFromBlocks { get; }
        byte LightFromSky { get; }
        IBiome Biome { get; }
        bool IsSolid { get; }
        bool IsDiggable { get; }
        double Hardness { get; }
        HarvestTier HarvestTier { get; }
        BlockMaterial Material { get; }
        bool IsDangerous { get; }
        bool IsEmpty { get; }

        IBlock WithLightFromBlocks(byte lightLevel);
        IBlock WithLightFromSky(byte lightLevel);
        IBlock WithBiome(IBiome biome);
        IBlock WithDataValue(byte metadata);
    }
}
