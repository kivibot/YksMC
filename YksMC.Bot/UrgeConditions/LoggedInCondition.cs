using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Behavior.Urge;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.UrgeConditions
{
    public class LoggedInCondition : IUrgeCondition
    {
        public string Name => "Logged in";

        public bool IsPossible(IWorld world)
        {
            return world.GetLocalPlayer() != null;
        }
    }
}
