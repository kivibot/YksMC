using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Client;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Nbt.Models;
using YksMC.Protocol.Packets.Play.Clientbound;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Behavior.Tasks.InventoryManagement
{
    public class SwapWindowSlotsTask : BehaviorTask<SwapWindowSlotsCommand>
    {
        public override string Name => $"SwapWindowSlots({_command.SourceSlot} <=> {_command.TargetSlot}, {_command.WindowId})";

        public SwapWindowSlotsTask(SwapWindowSlotsCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler)
            : base(command, minecraftClient, taskScheduler)
        {
        }

        public override bool IsPossible(IWorld world)
        {
            return world.Windows.ContainsWindow(_command.WindowId);
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {
            return Result(world);
        }

        public override async Task<bool?> OnStartAsync(IWorld world)
        {
            if (!await _taskScheduler.RunCommandAsync(new PickUpWindowSlotCommand(_command.WindowId, _command.SourceSlot)))
            {
                return false;
            }
            if (!await _taskScheduler.RunCommandAsync(new PickUpWindowSlotCommand(_command.WindowId, _command.TargetSlot)))
            {
                return false;
            }
            if (!await _taskScheduler.RunCommandAsync(new PickUpWindowSlotCommand(_command.WindowId, _command.SourceSlot)))
            {
                return false;
            }
            return true;
        }

        public override IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick)
        {
            return Result(world);
        }
    }
}
