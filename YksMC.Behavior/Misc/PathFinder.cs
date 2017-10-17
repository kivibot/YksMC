using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Misc
{
    public class PathFinder : IPathFinder
    {
        public IPathFindingResult FindPath(IBlockLocation startLocation, IBlockLocation endLocation, IDimension dimension)
        {
            Queue<IBlockLocation> queue = new Queue<IBlockLocation>();
            Dictionary<IBlockLocation, IBlockLocation> parents = new Dictionary<IBlockLocation, IBlockLocation>();
            parents[startLocation] = null;
            queue.Enqueue(startLocation);
            while (queue.Count > 0)
            {
                IBlockLocation location = queue.Dequeue();
                IEnumerable<IBlockLocation> neighbors = GetNeighbors(location, dimension);
                foreach (IBlockLocation neighbor in neighbors)
                {
                    if (parents.ContainsKey(neighbor))
                    {
                        continue;
                    }
                    parents[neighbor] = location;
                    if (neighbor.Equals(endLocation))
                    {
                        return GetPathFromDictionary(parents, endLocation);
                    }
                    queue.Enqueue(neighbor);
                }
            }
            return new PathFindingResult();
        }

        private IPathFindingResult GetPathFromDictionary(Dictionary<IBlockLocation, IBlockLocation> parents, IBlockLocation location)
        {
            List<IBlockLocation> path = new List<IBlockLocation>();
            while (location != null)
            {
                path.Add(location);
                location = parents[location];
            }
            path.Reverse();
            return new PathFindingResult(path);
        }

        private IEnumerable<IBlockLocation> GetNeighbors(IBlockLocation location, IDimension dimension)
        {
            IBlock block = dimension.GetBlock(location.Add(0, -1, 0));
            IBlockLocation[] neighbors;
            if (block.Type.IsSolid)
            {
                neighbors = new IBlockLocation[]{
                    location.Add(0, 0, -1),    //N
                    location.Add(1, 0, 0),     //E
                    location.Add(0, 0, 1),     //S
                    location.Add(-1, 0, 0),     //W
                };
            }
            else
            {
                neighbors = new IBlockLocation[]{
                    location.Add(0, -1, 0)     //D
                };
            }
            return neighbors.Where(loc => IsValidLocation(loc, dimension));
        }

        private bool IsValidLocation(IBlockLocation location, IDimension dimension)
        {
            //TODO: handle unloaded chunks
            if(!dimension.GetChunk(new ChunkCoordinate(location)).GetBlock(new BlockLocation(0, 0, 0)).Type.IsSolid)
            {
                return false;
            }
            IBlock midBlock = dimension.GetBlock(location);
            IBlock upperBlock = dimension.GetBlock(location.Add(0, 1, 0));
            return !midBlock.Type.IsSolid && !upperBlock.Type.IsSolid;
        }
    }
}
