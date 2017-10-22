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
using YksMC.Client;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks.Movement
{
    public class WalkToLocationTask : BehaviorTask<WalkToLocationCommand>
    {
        private readonly IPathFinder _pathFinder;

        public override string Name => $"WalkToLocation({_command.Location.X}, {_command.Location.Y}, {_command.Location.Z})";

        public WalkToLocationTask(WalkToLocationCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler, IPathFinder pathFinder)
            : base(command, minecraftClient, taskScheduler)
        {
            _pathFinder = pathFinder;
        }

        public override bool IsPossible(IWorld world)
        {
            IPlayer player = world.GetLocalPlayer();
            if (player == null)
            {
                return false;
            }
            if (!player.HasEntity)
            {
                return false;
            }
            IEntity entity = world.GetPlayerEntity();
            if (!entity.IsAlive)
            {
                return false;
            }
            return true;
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {
            return Result(world);
        }

        public override async Task<bool?> OnStartAsync(IWorld world)
        {
            return await WalkToLocationAsync(world); 
        }

        private async Task<bool> WalkToLocationAsync(IWorld world)
        {
            await Task.Yield();
            IEntity entity = world.GetPlayerEntity();
            IDimension dimension = world.GetCurrentDimension();
            IBlockLocation startLocation = new BlockLocation(entity.Location);
            IBlockLocation endLocation = _command.Location;

            IPathFindingResult result = _pathFinder.FindPath(startLocation, endLocation, dimension);

            if (result.Failed)
            {
                return false;
            }

            foreach (IPathWaypoint waypoint in result.Path)
            {
                IBlockLocation blockLocation = waypoint.Target;

                if (waypoint.MovementType == PathMovementType.Jump)
                {
                    if (!await JumpAsync())
                    {
                        return false;
                    }
                    continue;
                }
                if (waypoint.MovementType == PathMovementType.JumpTo)
                {
                    if (!await JumpAsync())
                    {
                        return false;
                    }
                }

                IEntityLocation location = new EntityLocation(blockLocation.X + 0.5, blockLocation.Y, blockLocation.Z + 0.5);
                bool success = await _taskScheduler.RunCommandAsync(new MoveToLocationCommand(location, 0.2));
                if (!success)
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> JumpAsync()
        {
            return await _taskScheduler.RunCommandAsync(new JumpCommand());
        }

        public override IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick)
        {
            return Result(world);
        }
    }
}
