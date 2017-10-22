using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
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
    public class OpenBlockWindowTask : BehaviorTask<OpenBlockWindowCommand>
    {
        private const int _timeout = 3 * 20;

        private int _ticksWaited = 0;

        public override string Name => $"OpenBlockWindow({_command.Location})";

        public OpenBlockWindowTask(OpenBlockWindowCommand command) : base(command)
        {
        }

        public override IWorldEventResult OnStart(IWorld world)
        {
            IContainerBlock block = world.GetCurrentDimension().GetBlock<IContainerBlock>(_command.Location);
            if (block == null || block.IsEmpty)
            {
                Fail();
                return Result(world);
            }
            if(world.Windows.GetNewestWindow().Id != 0)
            {
                Fail();
                return Result(world);
            }
            PlayerBlockPlacementPacket packet = new PlayerBlockPlacementPacket()
            {
                Location = new Position(_command.Location.X, _command.Location.Y, _command.Location.Z),
                Face = 0,
                Hand = 1,
                CursorX = 0,
                CursorY = 0,
                CursorZ = 0
            };
            return Result(world, packet);
        }

        public override void OnTick(IWorld world, IGameTick tick)
        {
            IWindow window = world.Windows.GetNewestWindow();
            if(window.Id != 0 && window.IsFilled)
            {
                Complete();
                return;
            }
            if(_ticksWaited > _timeout)
            {
                Fail();
                return;
            }
            _ticksWaited++;
            return;
        }
    }
}
