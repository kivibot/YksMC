using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YksMC.Behavior.Misc;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks.Movement
{
    public class WalkToLocationTask : BehaviorTask<WalkToLocationCommand>
    {
        private readonly IPathFinder _pathFinder;
        private readonly IBehaviorTaskScheduler _taskScheduler;

        public override string Name => $"WalkToLocation({_command.Location.X}, {_command.Location.Y}, {_command.Location.Z})";

        public WalkToLocationTask(WalkToLocationCommand command, IPathFinder pathFinder, IBehaviorTaskScheduler taskScheduler)
            : base(command)
        {
            _pathFinder = pathFinder;
            _taskScheduler = taskScheduler;
        }

        public override IWorldEventResult OnStart(IWorld world)
        {
            WalkToLocationAsync(world);
            return Result(world);
        }

        private async void WalkToLocationAsync(IWorld world)
        {
            await Task.Yield();
            IEntity entity = world.GetPlayerEntity();
            IDimension dimension = world.GetCurrentDimension();
            IBlockLocation startLocation = new BlockLocation(entity.Location);
            IBlockLocation endLocation = _command.Location;

            IPathFindingResult result = _pathFinder.FindPath(startLocation, endLocation, dimension);

            if (result.Failed)
            {
                Fail();
                return;
            }

            IEnumerable<IBlockLocation> path = OptimizePath(result.Path);

            foreach (IBlockLocation blockLocation in path)
            {
                IEntityLocation location = new EntityLocation(blockLocation.X + 0.5, blockLocation.Y, blockLocation.Z + 0.5);
                IBehaviorTask task = await _taskScheduler.RunTaskAsync(new MoveToLocationCommand(location, 0.2));
                if (task.IsFailed)
                {
                    Fail();
                    return;
                }
            }

            Complete();
        }

        private IEnumerable<IBlockLocation> OptimizePath(IReadOnlyList<IBlockLocation> path)
        {
            yield return path[0];
            for (int i = 0; i < path.Count - 2; i++)
            {
                IVector3<double> a = path[i].AsVector();
                IVector3<double> b = path[i + 1].AsVector();
                IVector3<double> c = path[i + 2].AsVector();
                IVector3<double> ab = b.Substract(a);
                IVector3<double> cb = b.Substract(c);
                if (ab.DotProduct(cb) != 0)
                {
                    continue;
                }
                yield return path[i + 1];
            }
            if (path.Count > 2)
            {
                yield return path[path.Count - 2];
            }
            if (path.Count > 1)
            {
                yield return path[path.Count - 1];
            }
        }

        public override void OnTick(IWorld world, IGameTick tick)
        {
            return;
        }
    }
}
