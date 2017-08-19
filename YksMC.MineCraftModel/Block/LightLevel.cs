using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Block
{
    public class LightLevel : ILightLevel
    {
        private readonly int _value;

        public int Value => _value;

        public LightLevel(int value)
        {
            _value = value;
        }
    }
}
