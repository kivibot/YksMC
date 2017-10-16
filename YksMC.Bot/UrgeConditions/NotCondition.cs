using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Behavior.Urge;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.UrgeConditions
{
    public class NotCondition : IUrgeCondition
    {
        private readonly IUrgeCondition _urgeCondition;

        public string Name => $"Not ({_urgeCondition.Name})";

        public NotCondition(IUrgeCondition urgeCondition)
        {
            _urgeCondition = urgeCondition;
        }

        public bool IsPossible(IWorld world)
        {
            return !_urgeCondition.IsPossible(world);
        }
    }
}
