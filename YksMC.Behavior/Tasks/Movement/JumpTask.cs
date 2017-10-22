using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks.Movement
{
    public class JumpTask : BehaviorTask<JumpCommand>
    {
        private const double _jumpVelocity = 0.5;

        public override string Name => "Jump";

        public JumpTask(JumpCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler) 
            : base(command, minecraftClient, taskScheduler)
        {
        }
        
        public override bool IsPossible(IWorld world)
        {
            IEntity entity = world.GetPlayerEntity();
            if (!entity.IsOnGround)
            {
                return false;
            }
            return true;
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {
            IEntity entity = world.GetPlayerEntity();
            entity = entity.ChangeVelocity(new Vector3d(0, _jumpVelocity, 0));
            return Success(world.ChangeCurrentDimension(d => d.ReplaceEntity(entity)));
        }

        public override IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick)
        {
            return Result(world);
        }
    }
}
