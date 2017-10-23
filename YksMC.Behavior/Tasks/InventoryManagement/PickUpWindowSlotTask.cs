using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Client;
using YksMC.MinecraftModel.ItemStack;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Behavior.Tasks.InventoryManagement
{
    public class PickUpWindowSlotTask : BehaviorTask<PickUpWindowSlotCommand>
    {
        private const int _timeout = 5 * 20;

        public override string Name => $"PickUpWindowSlot(slot: {_command.Slot}, window: {_command.WindowId})";

        private IItemStack _startingSlotValue;
        private int _ticksWaited;

        public PickUpWindowSlotTask(PickUpWindowSlotCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler)
            : base(command, minecraftClient, taskScheduler)
        {
        }

        public override bool IsPossible(IWorld world)
        {
            return world.Windows.ContainsWindow(_command.WindowId);
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {
            _startingSlotValue = world.Windows.GetCursorWindow().Slots[0];
            if (world.Windows[_command.WindowId].Slots[_command.Slot].Equals(_startingSlotValue))
            {
                return Success(world);
            }
            _minecraftClient.SendPacket(new ClickEmptyWindowSlotPacket()
            {
                WindowId = (byte)_command.WindowId,
                Slot = (short)_command.Slot,
                Mode = WindowClickMode.Click,
                Button = 0,
                ActionNumber = 0
            });
            return Result(world);
        }

        public override IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick)
        {
            if (!IsPossible(world))
            {
                return Failure(world);
            }
            if (!world.Windows.GetCursorWindow().Slots[0].Equals(_startingSlotValue))
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
