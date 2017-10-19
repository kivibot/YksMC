using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Urge;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.UrgeConditions
{
    public class SolidBlockCondition : IUrgeCondition
    {
        private readonly IBlockLocation _location;

        public string Name => $"IsSolid({_location.X}, {_location.Y}, {_location.Z})";

        public SolidBlockCondition(IBlockLocation location)
        {
            _location = location;
        }

        public bool IsPossible(IWorld world)
        {
            return world.GetCurrentDimension().GetBlock(_location).Type.IsSolid;
        }
    }
}
