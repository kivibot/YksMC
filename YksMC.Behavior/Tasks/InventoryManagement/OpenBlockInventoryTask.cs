using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Window;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Behavior.Tasks.InventoryManagement
{
    /// <summary>
    /// Clicks the target block using the offhand which currently should not contain anything.
    /// </summary>
    public class OpenBlockInventoryTask : BehaviorTask<OpenBlockInventoryCommand>
    {
        private const int _timeout = 3 * 20;

        private int _ticksWaited = 0;

        public override string Name => $"OpenBlockWindow({_command.Location})";

        public OpenBlockInventoryTask(OpenBlockInventoryCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler) 
            : base(command, minecraftClient, taskScheduler)
        {
        }
        
        public override bool IsPossible(IWorld world)
        {
            IContainerBlock block = world.GetCurrentDimension().GetBlock<IContainerBlock>(_command.Location);
            if (block == null || block.IsEmpty)
            {
                return false;
            }
            if (world.Windows.GetNewestWindow().Id != 0)
            {
                return false;
            }
            return true;
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {
            IContainerBlock block = world.GetCurrentDimension().GetBlock<IContainerBlock>(_command.Location);

            PlayerBlockPlacementPacket packet = new PlayerBlockPlacementPacket()
            {
                Location = new Position(_command.Location.X, _command.Location.Y, _command.Location.Z),
                Face = 0,
                Hand = 1,
                CursorX = 0,
                CursorY = 0,
                CursorZ = 0
            };
            _minecraftClient.SendPacket(packet);

            return Result(world);
        }

        public override IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick)
        {
            IWindow window = world.Windows.GetNewestWindow();
            if(window.Id != 0 && window.IsFilled)
            {
                _taskScheduler.EnqueueCommand(new KeepBlockInventoryUpdatedCommand(_command.Location, window.Id));
                return Success(world);
            }
            if(_ticksWaited > _timeout)
            {
                return Failure(world);
            }
            _ticksWaited++;
            return Result(world);
        }
    }
}
