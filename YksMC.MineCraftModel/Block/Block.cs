using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Block
{
    public class Block : IBlock
    {
        private readonly BlockType _type;
        private readonly LightLevel _lightLevel;
        private readonly LightLevel _skylightLevel;
        private readonly Biome _biome;

        public IBlockType Type => _type;
        public ILightLevel LightLevel => LightLevel;
        public ILightLevel SkylightLevel => SkylightLevel;
        public IBiome Biome => Biome;

        public Block(BlockType type, LightLevel lightLevel, LightLevel skylightLevel, Biome biome)
        {
            _type = type;
            _lightLevel = lightLevel;
            _skylightLevel = skylightLevel;
            _biome = biome;
        }

        public IBlock ChangeLightLevels(LightLevel lightLevel, LightLevel skylightLevel)
        {
            return new Block(_type, lightLevel, skylightLevel, _biome);
        }

        public IBlock ChangeType(BlockType type)
        {
            return new Block(type, _lightLevel, _skylightLevel, _biome);
        }

        public IBlock ChangeBiome(Biome biome)
        {
            return new Block(_type, _lightLevel, _skylightLevel, biome);
        }
    }
}
