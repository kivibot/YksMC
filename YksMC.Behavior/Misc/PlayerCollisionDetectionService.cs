using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Misc
{
    public class PlayerCollisionDetectionService : IPlayerCollisionDetectionService
    {
        private const double _hitboxWidth = 0.625;
        private const double _hitboxHeight = 1.74;
        private const double _hitboxOffset = 0.000;

        public IEntity UpdatePlayerPosition(IWorld world, IVector3<double> velocity)
        {
            IPlayer player = world.GetLocalPlayer();
            if (player == null || !player.HasEntity)
            {
                throw new ArgumentException("Player not spawned!");
            }
            IDimension dimension = world.GetCurrentDimension();
            IEntity entity = dimension.Entities[player.EntityId];

            Hit firstHit = GetLongestValidLinearMovement(dimension, entity.Location, velocity);
            IEntityLocation endLocation = entity.Location.Add(velocity.Multiply(firstHit.Distance));
            bool isOnGround = firstHit.HitY && velocity.Y < 0 && !firstHit.IsNotSolid;

            entity = entity.ChangeLocation(endLocation)
                .ChangeOnGround(isOnGround);

            return entity;
        }

        private Hit GetLongestValidLinearMovement(IDimension dimension, IEntityLocation entityLocation, IVector3<double> velocity)
        {
            IReadOnlyList<IEntityLocation> hitboxPoints = GetRelevantHitboxPoints(velocity, entityLocation);
            if (hitboxPoints.Count == 0)
            {
                return new Hit() { Distance = 1, IsNotSolid = true };
            }
            return hitboxPoints.Select(location => GetFirstHit(dimension, location, velocity)).OrderBy(h => h.Distance).First();
        }

        private Hit GetFirstHit(IDimension dimension, IEntityLocation startLocation, IVector3<double> velocity)
        {
            IEnumerable<Hit> hits = RayCast(startLocation, velocity);

            foreach (Hit hit in hits)
            {
                IEntityLocation location = startLocation.Add(velocity.Multiply(hit.Distance));
                IBlock block = GetBlock(dimension, location, velocity, hit);
                if (IsSolidBlock(block))
                {
                    return hit;
                }
            }

            return new Hit() { Distance = 1, IsNotSolid = true };
        }

        private IEnumerable<Hit> RayCast(IEntityLocation location, IVector3<double> ray)
        {
            List<Hit> hits = new List<Hit>();
            double rayLength = ray.Length();
            foreach (double scalarDistance in RayCastScalar(location.X, ray.X))
            {
                hits.Add(new Hit()
                {
                    Distance = scalarDistance * rayLength,
                    HitX = true
                });
            }
            foreach (double scalarDistance in RayCastScalar(location.Y, ray.Y))
            {
                hits.Add(new Hit()
                {
                    Distance = scalarDistance * rayLength,
                    HitY = true
                });
            }
            foreach (double scalarDistance in RayCastScalar(location.Z, ray.Z))
            {
                hits.Add(new Hit()
                {
                    Distance = scalarDistance * rayLength,
                    HitZ = true
                });
            }
            return hits.OrderBy(hit => hit.Distance);
        }

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
                yield return distance / Math.Abs(ray);
                current = current + step;
            }
        }

        private IBlock GetBlock(IDimension dimension, IEntityLocation entityLocation, IVector3<double> velocity, Hit hit)
        {
            int blockX = GetBlockCoordinateScalar(entityLocation.X, velocity.X, hit.HitX);
            int blockY = GetBlockCoordinateScalar(entityLocation.Y, velocity.Y, hit.HitY);
            int blockZ = GetBlockCoordinateScalar(entityLocation.Z, velocity.Z, hit.HitZ);
            return dimension.GetBlock(new BlockLocation(blockX, blockY, blockZ));
        }

        private int GetBlockCoordinateScalar(double location, double direction, bool hitAxis)
        {
            if (hitAxis)
            {
                if (direction < 0)
                {
                    return (int)Math.Floor(location) - 1;
                }
                else
                {
                    return (int)Math.Floor(location) + 1;
                }
            }
            return (int)Math.Floor(location);
        }

        private IReadOnlyList<IEntityLocation> GetRelevantHitboxPoints(IVector3<double> velocity, IEntityLocation entityLocation)
        {
            List<IEntityLocation> hitboxCorners = new List<IEntityLocation>();

            hitboxCorners.Add(entityLocation.Add(new Vector3d(0.5 * _hitboxWidth, _hitboxOffset, 0.5 * _hitboxWidth)));
            hitboxCorners.Add(entityLocation.Add(new Vector3d(-0.5 * _hitboxWidth, _hitboxOffset, 0.5 * _hitboxWidth)));
            hitboxCorners.Add(entityLocation.Add(new Vector3d(0.5 * _hitboxWidth, _hitboxOffset, -0.5 * _hitboxWidth)));
            hitboxCorners.Add(entityLocation.Add(new Vector3d(-0.5 * _hitboxWidth, _hitboxOffset, -0.5 * _hitboxWidth)));

            hitboxCorners.Add(entityLocation.Add(new Vector3d(0.5 * _hitboxWidth, _hitboxOffset + _hitboxHeight, 0.5 * _hitboxWidth)));
            hitboxCorners.Add(entityLocation.Add(new Vector3d(-0.5 * _hitboxWidth, _hitboxOffset + _hitboxHeight, 0.5 * _hitboxWidth)));
            hitboxCorners.Add(entityLocation.Add(new Vector3d(0.5 * _hitboxWidth, _hitboxOffset + _hitboxHeight, -0.5 * _hitboxWidth)));
            hitboxCorners.Add(entityLocation.Add(new Vector3d(-0.5 * _hitboxWidth, _hitboxOffset + _hitboxHeight, -0.5 * _hitboxWidth)));

            return hitboxCorners;
        }

        private bool IsSolidBlock(IBlock block)
        {
            return block.IsSolid;
        }

        private class Hit
        {
            public double Distance { get; set; }
            public bool HitX { get; set; }
            public bool HitY { get; set; }
            public bool HitZ { get; set; }
            public bool IsNotSolid { get; set; }
        }
    }
}
