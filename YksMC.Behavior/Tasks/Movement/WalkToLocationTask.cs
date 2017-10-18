using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YksMC.Behavior.Misc;
using YksMC.Behavior.Misc.Pathfinder;
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
            
            foreach (IPathWaypoint waypoint in result.Path)
            {
                IBlockLocation blockLocation = waypoint.Target;

                if(waypoint.MovementType == PathMovementType.Jump)
                {
                    if (!await JumpAsync())
                    {
                        return;
                    }
                    continue;
                }

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

        private async Task<bool> JumpAsync()
        {
            IBehaviorTask task = await _taskScheduler.RunTaskAsync(new JumpCommand());
            if (task.IsFailed)
            {
                Fail();
                return false;
            }
            return true;
        }

        public override void OnTick(IWorld world, IGameTick tick)
        {
            return;
        }
    }
}
