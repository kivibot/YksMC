using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Behavior.Misc;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks.Movement
{
    public class MoveLinearTask : BehaviorTask<MoveLinearCommand>
    {
        private readonly IPlayerCollisionDetectionService _playerCollisionDetectionService;
        private readonly IBehaviorTaskScheduler _taskScheduler;

        public override string Name => "MoveLinear";

        public MoveLinearTask(MoveLinearCommand command, IPlayerCollisionDetectionService playerCollisionDetectionService, IBehaviorTaskScheduler taskScheduler)
            : base(command)
        {
            _command = command;
            _playerCollisionDetectionService = playerCollisionDetectionService;
            _taskScheduler = taskScheduler;
        }

        public override IWorldEventResult OnStart(IWorld world)
        {
            IEntity entity = world.GetPlayerEntity();

            if(entity == null)
            {
                Fail();
                return Result(world);
            }

            IVector3<double> vector = _command.Movement;
            
            entity = _playerCollisionDetectionService.UpdatePlayerPosition(world, vector);

            Complete();
            return Result(world.ChangeCurrentDimension(d => d.ReplaceEntity(entity)));
        }

        public override void OnTick(IWorld world, IGameTick tick)
        {

        }
    }
}
