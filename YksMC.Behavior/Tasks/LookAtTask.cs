using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks
{
    public class LookAtTask : BehaviorTask<LookAtCommand>
    {
        public override string Name => $"LookAt({_command.Location.X}, {_command.Location.Y}, {_command.Location.Z})";

        public LookAtTask(LookAtCommand command) : base(command)
        {
        }

        public override IWorldEventResult OnStart(IWorld world)
        {
            IEntity localEntity = world.GetPlayerEntity()
                .LookAt(_command.Location);
            Complete();
            return Result(world.ChangeCurrentDimension(dimension => dimension.ReplaceEntity(localEntity)));
        }

        public override void OnTick(IWorld world, IGameTick tick)
        {
            return;
        }
    }
}
