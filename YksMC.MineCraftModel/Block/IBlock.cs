using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Block
{
    public interface IBlock
    {
        IBlockType Type { get; }
        ILightLevel LightLevel { get; }
        ILightLevel SkylightLevel { get; }
        IBiome Biome { get; }

        IBlock ChangeLightLevels(LightLevel lightLevel, LightLevel skylightLevel);
        IBlock ChangeType(BlockType type);
        IBlock ChangeBiome(Biome biome);
    }
}
