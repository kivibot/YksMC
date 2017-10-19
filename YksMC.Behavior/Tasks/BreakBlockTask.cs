using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.BlockType;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Behavior.Tasks
{
    public class BreakBlockTask : BehaviorTask<BreakBlockCommand>
    {
        private const int _ticksPerSecond = 20;
        private const int _timeout = 2 * _ticksPerSecond;
        private const double _flyingMultiplier = 5;
        private const double _harvestableMultiplier = 1.5;
        private const double _unharvestableMultiplier = 5;

        private readonly IBehaviorTaskScheduler _taskScheduler;

        private int _ticksWaited;
        private int _ticksNeeded;

        public override string Name => $"BreakBlock({_command.Location.X}, {_command.Location.Y}, {_command.Location.Z})";

        public BreakBlockTask(BreakBlockCommand command, IBehaviorTaskScheduler taskScheduler) : base(command)
        {
            _taskScheduler = taskScheduler;
            _ticksWaited = 0;
        }

        public override IWorldEventResult OnStart(IWorld world)
        {
            IBlock block = world.GetCurrentDimension().GetBlock(_command.Location);
            if (!block.Type.IsDiggable)
            {
                Fail();
                return Result(world);
            }
            IEntity entity = world.GetPlayerEntity();

            CalculateTicksNeeded(block.Type);

            PlayerDiggingPacket startPacket = new PlayerDiggingPacket()
            {
                Status = PlayerDiggingStatus.StartedDigging,
                Location = new Position(_command.Location.X, _command.Location.Y, _command.Location.Z),
                Face = 0,
            };
            PlayerDiggingPacket endPacket = new PlayerDiggingPacket()
            {
                Status = PlayerDiggingStatus.FinishedDigging,
                Location = new Position(_command.Location.X, _command.Location.Y, _command.Location.Z),
                Face = 0,
            };

            _taskScheduler.EnqueueTask(new LookAtCommand(new EntityLocation(_command.Location.X + 0.5, _command.Location.Y, _command.Location.Z + 0.5)));

            return Result(world, startPacket, endPacket);
        }

        public override void OnTick(IWorld world, IGameTick tick)
        {
            if (_ticksWaited == _ticksNeeded)
            {
                //TODO: check if worked
                Complete();
                return;
            }
            if (_ticksWaited > _ticksNeeded + _timeout)
            {
                Fail();
                return;
            }
            _ticksWaited++;
        }

        private void CalculateTicksNeeded(IBlockType blockType)
        {
            double ticks = _unharvestableMultiplier * blockType.Hardness * _ticksPerSecond;
            _ticksNeeded = (int)Math.Ceiling(ticks);
        }
    }
}
