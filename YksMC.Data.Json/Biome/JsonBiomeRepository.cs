using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.Biome;

namespace YksMC.Data.Json.Biome
{
    public class JsonBiomeRepository : IBiomeRepository
    {
        private readonly IReadOnlyDictionary<int, IBiome> _biomes;

        public JsonBiomeRepository()
        {
            _biomes = JsonConvert.DeserializeObject<List<JsonBiome>>(Resources.Biomes)
                .ToDictionary(b => b.Id, b => (IBiome)new MinecraftModel.Biome.Biome(b.Name));
        }

        public IBiome GetBiome(int biomeId)
        {
            return _biomes[biomeId];
        }
    }
}
