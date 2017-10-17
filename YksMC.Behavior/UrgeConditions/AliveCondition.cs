using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Urge;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.UrgeConditions
{
    public class AliveCondition : IUrgeCondition
    {
        public string Name => "Alive";

        public bool IsPossible(IWorld world)
        {
            IPlayer player = world.GetLocalPlayer();
            if(player == null || !player.HasEntity)
            {
                return false;
            }
            IEntity entity = world.GetPlayerEntity();
            return entity.IsAlive;
        }
    }
}
