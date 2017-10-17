using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks
{
    public class LookAtNearestPlayerTask : IBehaviorTask
    {
        private bool _isCompleted;
        private bool _isFailed;

        public string Name => "LookAtNearestPlayer";
        public bool IsCompleted => _isCompleted;
        public bool IsFailed => _isFailed;

        public IWorld OnStart(IWorld world)
        {
            IPlayer localPlayer = world.GetLocalPlayer();
            if (localPlayer == null)
            {
                Fail();
                return world;
            }
            if (!localPlayer.HasEntity)
            {
                Fail();
                return world;
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
                return world;
            }

            IVector3<double> lookVector = nearest.AsVector().Substract(localEntity.Location.AsVector());
            if (lookVector == Vector3d.Zero)
            {
                Complete();
                return world;
            }


            double pitch = -Math.Atan2(lookVector.Y, new Vector3d(lookVector.X, 0, lookVector.Z).Length());
            double yaw = -Math.Atan2(lookVector.X, lookVector.Z);

            localEntity = localEntity.ChangeLook(yaw, pitch);

            Complete();
            return world.ReplaceCurrentDimension(dimension.ReplaceEntity(localEntity));
        }

        private void Fail()
        {
            _isFailed = true;
            _isCompleted = true;
        }

        private void Complete()
        {
            _isCompleted = true;
        }

        public void OnTick(IWorld world)
        {
            return;
        }
    }
}
