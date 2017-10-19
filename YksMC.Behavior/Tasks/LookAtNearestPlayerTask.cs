using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks
{
    public class LookAtNearestPlayerTask : BehaviorTask<LookAtNearestPlayerCommand>
    {
        private readonly IBehaviorTaskScheduler _taskScheduler;

        public override string Name => "LookAtNearestPlayer";

        public LookAtNearestPlayerTask(LookAtNearestPlayerCommand command, IBehaviorTaskScheduler taskScheduler)
            : base(command)
        {
            _taskScheduler = taskScheduler;
        }

        public override IWorldEventResult OnStart(IWorld world)
        {
            IPlayer localPlayer = world.GetLocalPlayer();
            if (localPlayer == null)
            {
                Fail();
                return Result(world);
            }
            if (!localPlayer.HasEntity)
            {
                Fail();
                return Result(world);
            }
            IDimension dimension = world.GetCurrentDimension();
            IEntity localEntity = dimension.Entities[localPlayer.EntityId];
            IEntityLocation nearest = world.GetPlayers()
                .Where(player => player.Id != localPlayer.Id && player.HasEntity && player.DimensionId == localPlayer.DimensionId)
                .Select(player => dimension.Entities[player.EntityId].Location)
                .OrderBy(location => localEntity.Location.AsVector().Substract(location.AsVector()).Length())
                .FirstOrDefault();
            if (nearest == null)
            {
                Fail();
                return Result(world);
            }

            PerformLookAsync(nearest);
            return Result(world);
        }

        private async void PerformLookAsync(IEntityLocation location)
        {
            if((await _taskScheduler.RunTaskAsync(new LookAtCommand(location))).IsFailed)
            {
                Fail();
            }
            Complete();
        }

        public override void OnTick(IWorld world, IGameTick tick)
        {
            return;
        }

    }
}
