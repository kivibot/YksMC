using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Behavior.Tasks.Building
{
    public class PlaceHeldBlockTask : BehaviorTask<PlaceHeldBlockCommand>
    {
        private const int _timeout = 3 * 20;

        private int _ticksWaited;

        public override string Name => $"PlaceHeldBlock({_command.Location})";

        public PlaceHeldBlockTask(PlaceHeldBlockCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler) 
            : base(command, minecraftClient, taskScheduler)
        {
        }

        public override bool IsPossible(IWorld world)
        {
            IBlock block = world.GetCurrentDimension().GetBlock<IBlock>(_command.Location);
            if (!block.IsEmpty)
            {
                return false;
            }
            return true;
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {
            IBlock block = world.GetCurrentDimension().GetBlock<IBlock>(_command.Location);
            PlayerBlockPlacementPacket packet = new PlayerBlockPlacementPacket()
            {
                Location = new Position(_command.Location.X, _command.Location.Y, _command.Location.Z),
                Face = 0,
                Hand = 0,
                CursorX = 0,
                CursorY = 0,
                CursorZ = 0
            };

            _minecraftClient.SendPacket(packet);

            return Result(world);
        }

        public override IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick)
        {
            IBlock block = world.GetCurrentDimension().GetBlock<IBlock>(_command.Location);
            if (!block.IsEmpty)
            {
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
