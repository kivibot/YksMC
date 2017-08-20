using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Biome;
using YksMC.MinecraftModel.BlockType;

namespace YksMC.MinecraftModel.Block
{
    public interface IBlock
    {
        IBlockType Type { get; }
        ILightLevel LightLevel { get; }
        ILightLevel SkylightLevel { get; }
        IBiome Biome { get; }

        IBlock ChangeLightLevels(ILightLevel lightLevel, ILightLevel skylightLevel);
        IBlock ChangeType(IBlockType type);
        IBlock ChangeBiome(IBiome biome);
    }
}
