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
        public override string Name => "LookAtNearestPlayer";

        public LookAtNearestPlayerTask(LookAtNearestPlayerCommand command)
            : base(command)
        {
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

            IVector3<double> lookVector = nearest.AsVector().Substract(localEntity.Location.AsVector());
            if (lookVector == Vector3d.Zero)
            {
                Complete();
                return Result(world);
            }


            double pitch = -Math.Atan2(lookVector.Y, new Vector3d(lookVector.X, 0, lookVector.Z).Length());
            double yaw = -Math.Atan2(lookVector.X, lookVector.Z);

            localEntity = localEntity.ChangeLook(yaw, pitch);

            Complete();
            return Result(world.ReplaceCurrentDimension(dimension.ReplaceEntity(localEntity)));
        }

        public override void OnTick(IWorld world, IGameTick tick)
        {
            return;
        }

    }
}
