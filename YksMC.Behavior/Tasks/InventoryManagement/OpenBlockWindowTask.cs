using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Dimension;
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
        public OpenBlockWindowTask(OpenBlockWindowCommand command) : base(command)
        {
        }

        public override string Name => throw new NotImplementedException();

        public override IWorldEventResult OnStart(IWorld world)
        {
            IBlock block = world.GetCurrentDimension().GetBlock(_command.Location);
            if (block.IsEmpty)
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
            return;
        }
    }
}
