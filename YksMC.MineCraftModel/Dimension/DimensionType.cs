using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Dimension
{
    public class DimensionType : IDimensionType
    {
        private readonly bool _hasSkylight;

        public bool HasSkylight => _hasSkylight;

        public DimensionType(bool hasSkylight)
        {
            _hasSkylight = hasSkylight;
        }
    }
}
