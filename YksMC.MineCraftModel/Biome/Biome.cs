using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace YksMC.MinecraftModel.Biome
{
    [DebuggerDisplay("{Name}")]
    public class Biome : IBiome
    {
        private readonly string _name;

        public string Name => _name;

        public Biome(string name)
        {
            _name = name;
        }
    }
}
