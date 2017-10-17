using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Behavior.Urge;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.UrgeScorers
{
    public class RandomScorer : IUrgeScorer
    {
        private readonly double _minValue;
        private readonly double _maxValue;
        private readonly Random _random;

        public string Name => $"Random[{Math.Round(_minValue, 2)}, {Math.Round(_maxValue, 2)}]";

        public RandomScorer(double minValue, double maxValue, Random random)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            _random = random;
        }

        public double GetScore(IWorld world)
        {
            return _minValue + (_maxValue - _minValue) * _random.NextDouble();
        }
    }
}
