using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Client;
using YksMC.MinecraftModel.Inventory;
using YksMC.MinecraftModel.ItemStack;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.Window;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks.InventoryManagement
{
    public class KeepPlayerInventoryUpdatedTask : BehaviorTask<KeepPlayerInventoryUpdatedCommand>
    {
        private const int _playerInventoryUniqueItems = 10;

        public override string Name => "KeepPlayerInventoryUpdated";

        public KeepPlayerInventoryUpdatedTask(KeepPlayerInventoryUpdatedCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler)
            : base(command, minecraftClient, taskScheduler)
        {
            ChangePriority(BehaviorTaskPriority.High);
        }

        public override bool IsPossible(IWorld world)
        {
            return world.GetLocalPlayer() != null && world.Windows.GetNewestWindow() != null;
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {
            return UpdateInventory(world);
        }

        public override IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick)
        {
            return UpdateInventory(world);
        }

        private IBehaviorTaskEventResult UpdateInventory(IWorld world)
        {
            if (!IsPossible(world))
            {
                return Failure(world);
            }
            IWindow window = world.Windows.GetNewestWindow();
            IPlayer player = world.GetLocalPlayer();
            IPlayerInventory inventory = player.GetInventory();
            for (int i = _playerInventoryUniqueItems; i < inventory.SlotCount; i++)
            {
                IItemStack stack = window.Slots[window.UniqueSlotCount + i - _playerInventoryUniqueItems];
                inventory = (IPlayerInventory)inventory.ChangeSlot(i, stack);
            }
            player = player.ChangeInvetory(inventory);
            world = world.ReplaceLocalPlayer(player);
            return Result(world);
        }
    }
}
