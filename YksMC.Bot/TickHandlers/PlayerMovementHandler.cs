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
        private const double _acceleration = -0.08;
        private const double _drag = 0.02;
        private const double _dragFactor = 1.0 - _drag;
        private const double _maxVelocity = 3.92;

        private const double _hitboxWidth = 0.3;
        private const double _hitboxHeight = 1.74;

        private const double _stepHeight = 0.6;

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
            double movedLength = GetLongestValidLinearMovement(dimension, entity.Location, velocity);
            IEntityLocation endLocation = entity.Location.Add(velocity.Multiply(movedLength));
            bool isOnGround = movedLength < 1;
            if (isOnGround)
            {
                velocity = new Vector3d(0, 0, 0);
            }

            entity = entity.ChangeLocation(endLocation)
                .ChangeVelocity(velocity)
                .ChangeOnGround(isOnGround);

            return Result(world.ReplaceCurrentDimension(dimension.ChangeEntity(entity)));
        }

        private double GetLongestValidLinearMovement(IDimension dimension, IEntityLocation entityLocation, IVector3<double> velocity)
        {
            IReadOnlyList<IEntityLocation> hitboxPoints = GetRelevantHitboxPoints(velocity, entityLocation);
            if (hitboxPoints.Count == 0)
            {
                return 1;
            }
            return hitboxPoints.Select(location => GetFirstHit(dimension, location, velocity)).Min();
        }

        private double GetFirstHit(IDimension dimension, IEntityLocation startLocation, IVector3<double> velocity)
        {
            IReadOnlyList<double> hits = RayCast(startLocation, velocity);

            foreach (double hitDistance in hits)
            {
                IEntityLocation location = startLocation.Add(velocity.Multiply(hitDistance));
                IBlock block = GetBlock(dimension, location, velocity);
                if (IsSolidBlock(block))
                {
                    return hitDistance;
                }
            }

            return 1;
        }

        private IReadOnlyList<double> RayCast(IEntityLocation location, IVector3<double> ray)
        {
            List<double> hits = new List<double>();
            double rayLength = ray.Length();
            foreach (double scalarDistance in RayCastScalar(location.X, ray.X))
            {
                hits.Add(scalarDistance * rayLength / Math.Abs(ray.X));
            }
            foreach (double scalarDistance in RayCastScalar(location.Y, ray.Y))
            {
                hits.Add(scalarDistance * rayLength / Math.Abs(ray.Y));
            }
            foreach (double scalarDistance in RayCastScalar(location.Z, ray.Z))
            {
                hits.Add(scalarDistance * rayLength / Math.Abs(ray.Z));
            }
            hits.Sort();
            return hits;
        }

        //TODO: should this returns values from 0 to 1 instead of 0 to ray
        private IEnumerable<double> RayCastScalar(double start, double ray)
        {
            if (ray == 0)
            {
                yield break;
            }
            double step;
            double current;
            if (ray > 0)
            {
                step = 1;
                current = Math.Ceiling(start);
            }
            else
            {
                step = -1;
                current = Math.Floor(start);
            }
            double distance;
            while ((distance = Math.Abs(current - start)) < Math.Abs(ray))
            {
                yield return distance;
                current = current + step;
            }
        }

        private IBlock GetBlock(IDimension dimension, IEntityLocation entityLocation, IVector3<double> velocity)
        {
            int blockX = GetBlockCoordinateScalar(entityLocation.X, velocity.X);
            int blockY = GetBlockCoordinateScalar(entityLocation.Y, velocity.Y);
            int blockZ = GetBlockCoordinateScalar(entityLocation.Z, velocity.Z);
            return dimension.GetBlock(new BlockCoordinate(blockX, blockY, blockZ));
        }

        private int GetBlockCoordinateScalar(double location, double direction)
        {
            if (direction < 0)
            {
                return (int)Math.Floor(location) - 1;
            }
            return (int)Math.Floor(location);
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
            IVector3<double> velocity = new Vector3d(previousVelocity.X, previousVelocity.Y * _dragFactor + _acceleration, previousVelocity.Z);
            return velocity;
        }

        private bool IsSolidBlock(IBlock block)
        {
            return block.Type.Name != "air";
        }

    }
}
