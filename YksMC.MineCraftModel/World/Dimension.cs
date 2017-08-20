using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.World
{
    public class Dimension : IDimension
    {
        private readonly bool _hasSkylight;

        public bool HasSkylight => _hasSkylight;

        public Dimension(bool hasSkylight)
        {
            _hasSkylight = hasSkylight;
        }
    }
}
