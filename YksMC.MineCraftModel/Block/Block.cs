using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Biome;
using YksMC.MinecraftModel.BlockType;

namespace YksMC.MinecraftModel.Block
{
    public class Block : IBlock
    {
        private readonly IBlockType _type;
        private readonly ILightLevel _lightLevel;
        private readonly ILightLevel _skylightLevel;
        private readonly IBiome _biome;

        public IBlockType Type => _type;
        public ILightLevel LightLevel => LightLevel;
        public ILightLevel SkylightLevel => SkylightLevel;
        public IBiome Biome => Biome;

        public Block(IBlockType type, ILightLevel lightLevel, ILightLevel skylightLevel, IBiome biome)
        {
            _type = type;
            _lightLevel = lightLevel;
            _skylightLevel = skylightLevel;
            _biome = biome;
        }

        public IBlock ChangeLightLevels(ILightLevel lightLevel, ILightLevel skylightLevel)
        {
            return new Block(_type, lightLevel, skylightLevel, _biome);
        }

        public IBlock ChangeType(IBlockType type)
        {
            return new Block(type, _lightLevel, _skylightLevel, _biome);
        }

        public IBlock ChangeBiome(IBiome biome)
        {
            return new Block(_type, _lightLevel, _skylightLevel, biome);
        }
    }
}
