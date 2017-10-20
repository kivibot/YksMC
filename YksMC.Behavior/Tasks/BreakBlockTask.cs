using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.GameObjectRegistry;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.BlockType;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.ItemStack;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Behavior.Tasks
{
    public class BreakBlockTask : BehaviorTask<BreakBlockCommand>
    {
        private const int _ticksPerSecond = 20;
        private const int _timeout = 2 * _ticksPerSecond;

        private readonly IBehaviorTaskScheduler _taskScheduler;
        private readonly IHarvestingTool _handTool;

        private int _ticksWaited;
        private int _ticksNeeded;

        public override string Name => $"BreakBlock({_command.Location.X}, {_command.Location.Y}, {_command.Location.Z})";

        public BreakBlockTask(BreakBlockCommand command, IBehaviorTaskScheduler taskScheduler, IGameObjectRegistry<IItemStack> items)
            : base(command)
        {
            _taskScheduler = taskScheduler;
            _handTool = items.Get<IHarvestingTool>("yksmc:hand");
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
            IPlayer player = world.GetLocalPlayer();
            IHarvestingTool tool = player.GetInventory().GetHeldItem<IHarvestingTool>() ?? _handTool;

            CalculateTicksNeeded(block.Type, tool);

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
            IBlock block = world.GetCurrentDimension().GetBlock(_command.Location);
            if (block.Type.Name == "air")
            {
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

        private void CalculateTicksNeeded(IBlockType blockType, IHarvestingTool tool)
        {
            double ticks = blockType.Hardness * _ticksPerSecond / tool.GetBreakingSpeed(blockType);
            _ticksNeeded = (int)Math.Ceiling(ticks);
        }
    }
}
