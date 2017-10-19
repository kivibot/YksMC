using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Urge;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.UrgeScorers
{
    public class DistanceScorer : IUrgeScorer
    {
        private readonly IEntityLocation _location;
        private readonly double _factor;

        public string Name => $"DistanceTo({_location.X}, {_location.Y}, {_location.Z})";

        public DistanceScorer(IEntityLocation location, double factor)
        {
            _location = location;
            _factor = factor;
        }

        public double GetScore(IWorld world)
        {
            IEntity entity = world.GetPlayerEntity();
            IVector3<double> delta = entity.Location.AsVector().Substract(_location.AsVector());
            return _factor * delta.Length();
        }
    }
}
