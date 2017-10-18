using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks.Movement
{
    public class JumpTask : BehaviorTask<JumpCommand>
    {
        private const double _jumpVelocity = 0.6;

        public override string Name => "Jump";

        public JumpTask(JumpCommand command) : base(command)
        {

        }

        public override IWorldEventResult OnStart(IWorld world)
        {
            IEntity entity = world.GetPlayerEntity();
            if (!entity.IsOnGround)
            {
                Fail();
                return Result(world);
            }
            entity = entity.ChangeVelocity(new Vector3d(0, _jumpVelocity, 0));
            Complete();
            return Result(world.ChangeCurrentDimension(d => d.ReplaceEntity(entity)));
        }

        public override void OnTick(IWorld world, IGameTick tick)
        {
            return;
        }
    }
}
