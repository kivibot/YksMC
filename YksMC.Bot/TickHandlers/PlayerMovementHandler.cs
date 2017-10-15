using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.TickHandlers
{
    public class PlayerMovementHandler : WorldEventHandler, IWorldEventHandler<IGameTick>
    {
        private const double _acceleration = 0.08;
        private const double _drag = 0.02;
        private const double _dragFactor = 1.0 - _drag;
        private const double _maxVelocity = 3.92;

        private const double _hitboxWidth = 0.3;
        private const double _hitboxHeight = 1.74;

        private readonly IVector3<double> _gravityDirection = new Vector3d(0, -1, 0);

        public IWorldEventResult ApplyEvent(IGameTick tick, IWorld world)
        {
            IPlayer player = world.GetLocalPlayer();
            if (!player.HasEntity)
            {
                return Result(world);
            }
            IDimension dimension = world.GetCurrentDimension();
            IEntity entity = dimension.GetEntity(player.EntityId);

            IVector3<double> velocity = GetNextVelocity(entity.Velocity);
            double movedLength = TraceAll(dimension, entity.Location, velocity);
            IEntityLocation endLocation = entity.Location.Add(velocity.Multiply(movedLength));
            bool isOnGround = movedLength < velocity.Length();
            if (isOnGround)
            {
                velocity = new Vector3d(0, 0, 0);
            }

            entity = entity.ChangeLocation(endLocation)
                .ChangeVelocity(velocity)
                .ChangeOnGround(isOnGround);

            return Result(world.ReplaceCurrentDimension(dimension.ChangeEntity(entity)));
        }

        private double TraceAll(IDimension dimension, IEntityLocation entityLocation, IVector3<double> velocity)
        {
            IReadOnlyList<IEntityLocation> hitboxPoints = GetRelevantHitboxPoints(velocity, entityLocation);
            if (hitboxPoints.Count == 0)
            {
                return 1;
            }
            return hitboxPoints.Select(location => TraceFull(dimension, location, velocity)).Min();
        }

        private double TraceFull(IDimension dimension, IEntityLocation startLocation, IVector3<double> velocity)
        {
            double tracedLength = 0;
            double maxLength = 1;
            IEntityLocation location = startLocation;

            while (tracedLength < maxLength)
            {
                double nextCollision = TraceSingleStep(location, velocity);
                tracedLength += nextCollision;
                tracedLength = Math.Min(maxLength, tracedLength);
                location = startLocation.Add(velocity.Multiply(tracedLength));

                IBlock block = GetBlock(dimension, location, velocity);
                if (block.Type.Name != "air")
                {
                    break;
                }
            }

            return tracedLength;
        }

        private double TraceSingleStep(IEntityLocation location, IVector3<double> ray)
        {
            double xDist = TraceSingleStepScalar(location.X, ray.X);
            double yDist = TraceSingleStepScalar(location.Y, ray.Y);
            double zDist = TraceSingleStepScalar(location.Z, ray.Z);
            double min = Math.Min(xDist, Math.Min(yDist, zDist));
            double factor;
            if (xDist == min)
            {
                factor = xDist / ray.X;
            }
            else if (yDist == min)
            {
                factor = yDist / ray.Y;
            }
            else
            {
                factor = zDist / ray.Z;
            }
            return ray.Multiply(factor).Length();
        }

        private double TraceSingleStepScalar(double start, double ray)
        {
            if (ray == 0)
            {
                return 1;
            }
            //TODO: expand
            return (ray < 0 ? (start == Math.Floor(start) ? -1 : 0) : (ray > 0 ? 1 : 0)) + Math.Floor(start) - start;
        }

        private IBlock GetBlock(IDimension dimension, IEntityLocation entityLocation, IVector3<double> velocity)
        {
            int blockX = (int)entityLocation.X;
            int blockY = (int)entityLocation.Y;
            int blockZ = (int)entityLocation.Z;

            return dimension.GetBlock(new BlockCoordinate(blockX, blockY, blockZ));
        }

        private IReadOnlyList<IEntityLocation> GetRelevantHitboxPoints(IVector3<double> velocity, IEntityLocation entityLocation)
        {
            List<IEntityLocation> hitboxCorners = new List<IEntityLocation>();

            hitboxCorners.Add(entityLocation.Add(new Vector3d(0.5 * _hitboxWidth, 0, 0.5 * _hitboxWidth)));
            hitboxCorners.Add(entityLocation.Add(new Vector3d(-0.5 * _hitboxWidth, 0, 0.5 * _hitboxWidth)));
            hitboxCorners.Add(entityLocation.Add(new Vector3d(0.5 * _hitboxWidth, 0, -0.5 * _hitboxWidth)));
            hitboxCorners.Add(entityLocation.Add(new Vector3d(-0.5 * _hitboxWidth, 0, -0.5 * _hitboxWidth)));

            hitboxCorners.Add(entityLocation.Add(new Vector3d(0.5 * _hitboxWidth, _hitboxHeight, 0.5 * _hitboxWidth)));
            hitboxCorners.Add(entityLocation.Add(new Vector3d(-0.5 * _hitboxWidth, _hitboxHeight, 0.5 * _hitboxWidth)));
            hitboxCorners.Add(entityLocation.Add(new Vector3d(0.5 * _hitboxWidth, _hitboxHeight, -0.5 * _hitboxWidth)));
            hitboxCorners.Add(entityLocation.Add(new Vector3d(-0.5 * _hitboxWidth, _hitboxHeight, -0.5 * _hitboxWidth)));

            return hitboxCorners;
        }

        private IVector3<double> GetNextVelocity(IVector3<double> previousVelocity)
        {
            IVector3<double> additionalVelocity = _gravityDirection.Multiply(_acceleration);
            IVector3<double> velocity = previousVelocity//.Multiply(_dragFactor)
                .Add(additionalVelocity);
            if (velocity.Length() > _maxVelocity)
            {
                //TODO: fix
                velocity = velocity.Normalize()
                    .Multiply(_maxVelocity);
            }
            return velocity;
        }
    }
}
