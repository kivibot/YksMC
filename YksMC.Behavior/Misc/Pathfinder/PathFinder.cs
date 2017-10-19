using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Misc.Pathfinder
{
    public class PathFinder : IPathFinder
    {
        private const double _fallingCost = 1.1;
        private const double _walkingCost = 1;
        private const double _jumpingCost = 2;
        private const double _costVariationFactor = 0.02;
        private readonly Random _random;

        public PathFinder(Random random)
        {
            _random = random;
        }

        public IPathFindingResult FindPath(IBlockLocation startLocation, IBlockLocation endLocation, IDimension dimension)
        {
            IPathfinderDatastructure<IBlockLocation, PathEdge> datastructure = new PathfinderDatastructure<IBlockLocation, PathEdge>();
            datastructure.AddOrUpdateIfSmaller(0, startLocation, new PathEdge(null, startLocation, PathMovementType.Walk, 0));

            while (!datastructure.IsEmpty)
            {
                CostEdgePair<PathEdge> pair = datastructure.PopFirst();

                if (pair.Edge.To.Equals(endLocation))
                {
                    return GetPath(datastructure, startLocation, endLocation);
                }

                foreach (PathEdge edge in GetEdges(pair.Edge.To, pair.Edge.MovementType, dimension))
                {
                    if (datastructure.IsVisited(edge.To))
                    {
                        continue;
                    }
                    double cumulativeCost = pair.Cost + edge.CumulativeCost + _costVariationFactor * _random.NextDouble();
                    datastructure.AddOrUpdateIfSmaller(cumulativeCost, edge.To, edge);
                }
            }

            return new PathFindingResult();
        }

        private IEnumerable<PathEdge> GetEdges(IBlockLocation blockLocation, PathMovementType previousMove, IDimension dimension)
        {
            if (previousMove != PathMovementType.Jump && !IsSolid(blockLocation.Add(0, -1, 0), dimension))
            {
                yield return new PathEdge(blockLocation, blockLocation.Add(0, -1, 0), PathMovementType.Fall, _fallingCost);
                yield break;
            }
            IBlockLocation[] neighbors = new IBlockLocation[]
            {
                blockLocation.Add(-1, 0, 0),
                blockLocation.Add(1, 0, 0),
                blockLocation.Add(0, 0, -1),
                blockLocation.Add(0, 0, 1)
            };
            foreach (IBlockLocation neighbor in neighbors)
            {
                if (!IsValidLocation(neighbor, dimension))
                {
                    continue;
                }
                yield return new PathEdge(blockLocation, neighbor, PathMovementType.Walk, _walkingCost);
            }
            IBlockLocation[] diagonalNeighbors = new IBlockLocation[]
            {
                blockLocation.Add(-1, 0, -1),
                blockLocation.Add(-1, 0, 1),
                blockLocation.Add(1, 0, -1),
                blockLocation.Add(1, 0, 1)
            };
            foreach (IBlockLocation neighbor in diagonalNeighbors)
            {
                IBlockLocation checkLocation = new BlockLocation(Math.Min(neighbor.X, blockLocation.X), blockLocation.Y, Math.Min(neighbor.Z, blockLocation.Z));
                if (!IsValidLocation(checkLocation, dimension, width: 2, requireSolidFloor: true))
                {
                    continue;
                }
                yield return new PathEdge(blockLocation, neighbor, PathMovementType.Walk, _walkingCost * Math.Sqrt(2));
            }
            IBlockLocation[] thirdDirectionNeighbors = new IBlockLocation[]
            {
                blockLocation.Add(-1, 0, 2),
                blockLocation.Add(1, 0, 2),
                blockLocation.Add(2, 0, 1),
                blockLocation.Add(2, 0, -1),
                blockLocation.Add(1, 0, -2),
                blockLocation.Add(-1, 0, -2),
                blockLocation.Add(-2, 0, -1),
                blockLocation.Add(-2, 0, 1)
            };
            foreach (IBlockLocation neighbor in thirdDirectionNeighbors)
            {
                IBlockLocation checkLocation = new BlockLocation(Math.Min(neighbor.X, blockLocation.X), blockLocation.Y, Math.Min(neighbor.Z, blockLocation.Z));
                if (!IsValidLocation(checkLocation, dimension, width: 3, requireSolidFloor: true))
                {
                    continue;
                }
                yield return new PathEdge(blockLocation, neighbor, PathMovementType.Walk, _walkingCost * Math.Sqrt(5));
            }
            if (IsValidLocation(blockLocation, dimension, height: 3, requireSolidFloor: true))
            {
                yield return new PathEdge(blockLocation, blockLocation.Add(0, 1, 0), PathMovementType.Jump, _jumpingCost);
            }


            if (IsValidLocation(blockLocation, dimension, height: 3, requireSolidFloor: true)
                && IsValidLocation(blockLocation.Add(-1, 0, 0), dimension, height: 3)
                && IsValidLocation(blockLocation.Add(-2, 0, 0), dimension, height: 3))
            {
                yield return new PathEdge(blockLocation, blockLocation.Add(-2, 0, 0), PathMovementType.JumpTo, _jumpingCost * 2);
            }
            if (IsValidLocation(blockLocation, dimension, height: 3, requireSolidFloor: true)
                && IsValidLocation(blockLocation.Add(1, 0, 0), dimension, height: 3)
                && IsValidLocation(blockLocation.Add(2, 0, 0), dimension, height: 3))
            {
                yield return new PathEdge(blockLocation, blockLocation.Add(2, 0, 0), PathMovementType.JumpTo, _jumpingCost * 2);
            }
            if (IsValidLocation(blockLocation, dimension, height: 3, requireSolidFloor: true)
                && IsValidLocation(blockLocation.Add(0, 0, -1), dimension, height: 3)
                && IsValidLocation(blockLocation.Add(0, 0, -2), dimension, height: 3))
            {
                yield return new PathEdge(blockLocation, blockLocation.Add(0, 0, -2), PathMovementType.JumpTo, _jumpingCost * 2);
            }
            if (IsValidLocation(blockLocation, dimension, height: 3, requireSolidFloor: true)
                && IsValidLocation(blockLocation.Add(0, 0, 1), dimension, height: 3)
                && IsValidLocation(blockLocation.Add(0, 0, 2), dimension, height: 3))
            {
                yield return new PathEdge(blockLocation, blockLocation.Add(0, 0, 2), PathMovementType.JumpTo, _jumpingCost * 2);
            }
        }

        private bool IsValidLocation(IBlockLocation location, IDimension dimension, int width = 1, bool requireSolidFloor = false, int height = 2)
        {
            //TODO: handle unloaded chunks
            if (!dimension.GetChunk(new ChunkCoordinate(location)).GetBlock(new BlockLocation(0, 0, 0)).Type.IsSolid)
            {
                return false;
            }
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < width; z++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        IBlock block = dimension.GetBlock(location.Add(x, y, z));
                        if (block.Type.IsSolid)
                        {
                            return false;
                        }
                    }
                    if (requireSolidFloor)
                    {
                        IBlock floorBlock = dimension.GetBlock(location.Add(x, -1, z));
                        if (!floorBlock.Type.IsSolid)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool IsSolid(IBlockLocation location, IDimension dimension)
        {
            IBlock block = dimension.GetBlock(location);
            return block.Type.IsSolid;
        }

        private IPathFindingResult GetPath(IPathfinderDatastructure<IBlockLocation, PathEdge> datastructure, IBlockLocation startLocation, IBlockLocation endLocation)
        {
            List<IPathWaypoint> path = new List<IPathWaypoint>();
            IBlockLocation location = endLocation;

            while (!location.Equals(startLocation))
            {
                PathEdge edge = datastructure.GetVisitEdge(location);

                path.Add(new PathWaypoint(edge.To, edge.MovementType));

                location = edge.From;
            }
            path.Reverse();

            return new PathFindingResult(path);
        }
    }
}
