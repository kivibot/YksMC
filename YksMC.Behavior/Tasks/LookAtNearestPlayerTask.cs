using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks
{
    public class LookAtNearestPlayerTask : BehaviorTask<LookAtNearestPlayerCommand>
    {
        public override string Name => "LookAtNearestPlayer";

        public LookAtNearestPlayerTask(LookAtNearestPlayerCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler) 
            : base(command, minecraftClient, taskScheduler)
        {
        }

        public override bool IsPossible(IWorld world)
        {
            IPlayer localPlayer = world.GetLocalPlayer();
            if (localPlayer == null)
            {
                return false;
            }
            if (!localPlayer.HasEntity)
            {
                return false;
            }
            return true;
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {            
            return Result(world);
        }

        public override async Task<bool?> OnStartAsync(IWorld world)
        {
            IPlayer localPlayer = world.GetLocalPlayer();
            IDimension dimension = world.GetCurrentDimension();
            IEntity localEntity = dimension.Entities[localPlayer.EntityId];
            IEntityLocation nearest = world.GetPlayers()
                .Where(player => player.Id != localPlayer.Id && player.HasEntity && player.DimensionId == localPlayer.DimensionId)
                .Select(player => dimension.Entities[player.EntityId].Location)
                .OrderBy(location => localEntity.Location.AsVector().Substract(location.AsVector()).Length())
                .FirstOrDefault();
            if (nearest == null)
            {
                return false;
            }
            
            if (!(await _taskScheduler.RunCommandAsync(new LookAtCommand(nearest))))
            {
                return false;
            }
            return true;
        }

        public override IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick)
        {
            return Result(world);
        }
    }
}
