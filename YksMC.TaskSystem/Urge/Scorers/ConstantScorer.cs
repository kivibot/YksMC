﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Urge.Scorers
{
    public class ConstantScorer : IUrgeScorer
    {
        private readonly double _value;

        public string Name => "Constant";

        public ConstantScorer(double value)
        {
            _value = value;
        }

        public double GetScore(IWorld world)
        {
            return _value;
        }
    }
}
