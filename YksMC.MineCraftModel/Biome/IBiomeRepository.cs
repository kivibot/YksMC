using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Biome
{
    public interface IBiomeRepository
    {
        IBiome GetBiome(int biomeId);
    }
}
