using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks
{
    public class LookAtTask : BehaviorTask<LookAtCommand>
    {
        public override string Name => $"LookAt({_command.Location})";

        public LookAtTask(LookAtCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler) 
            : base(command, minecraftClient, taskScheduler)
        {
        }
        
        public override bool IsPossible(IWorld world)
        {
            return true;
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {
            IEntity localEntity = world.GetPlayerEntity()
                .LookAt(_command.Location);
            return Success(world.ChangeCurrentDimension(dimension => dimension.ReplaceEntity(localEntity)));
        }

        public override IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick)
        {
            return Result(world);
        }
    }
}
