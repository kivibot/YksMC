using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Urge;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.UrgeConditions
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
