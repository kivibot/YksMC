using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Behavior.Misc;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks.Movement
{
    public class MoveLinearTask : BehaviorTask<MoveLinearCommand>
    {
        private readonly IPlayerCollisionDetectionService _playerCollisionDetectionService;

        public override string Name => "MoveLinear";

        public MoveLinearTask(MoveLinearCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler, IPlayerCollisionDetectionService playerCollisionDetectionService) 
            : base(command, minecraftClient, taskScheduler)
        {
            _playerCollisionDetectionService = playerCollisionDetectionService;
        }

        public override bool IsPossible(IWorld world)
        {
            IEntity entity = world.GetPlayerEntity();
            if (entity == null)
            {
                return false;
            }
            return true;
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {
            IEntity entity = world.GetPlayerEntity();
            IVector3<double> vector = _command.Movement;            
            entity = _playerCollisionDetectionService.UpdatePlayerPosition(world, vector);
            world = world.ChangeCurrentDimension(d => d.ReplaceEntity(entity));
            return Success(world);
        }

        public override IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick)
        {
            return Result(world);
        }
    }
}
