using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.GameObjectRegistry;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Inventory;
using YksMC.MinecraftModel.ItemStack;
using YksMC.MinecraftModel.ItemType;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.Window;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Behavior.PacketHandlers
{
    public class WindowPacketHandler : WorldEventHandler, IWorldEventHandler<WindowItemsPacket>, IWorldEventHandler<SetWindowSlotPacket>
    {
        private readonly IGameObjectRegistry<IItemStack> _itemStacks;

        public WindowPacketHandler(IGameObjectRegistry<IItemStack> itemStacks)
        {
            _itemStacks = itemStacks;
        }

        public IWorldEventResult Handle(IWorldEvent<WindowItemsPacket> message)
        {
            WindowItemsPacket packet = message.Event;
            IWorld world = message.World;
            IPlayer player = world.GetLocalPlayer();
            IPlayerInventory inventory = player.GetInventory();

            for (int index = 0; index < packet.Slots.Count; index++)
            {
                WindowSlotData slotData = packet.Slots.Values[index];
                IItemStack itemStack = _itemStacks.Get(slotData.BlockId)
                    .ChangeSize(slotData.ItemCount);
                //.ChangeDurability(slotData.ItemDamage);

                inventory = (IPlayerInventory)inventory.ChangeSlot(index, itemStack);
            }

            player = player.ChangeInvetory(inventory);
            world = world.ReplaceLocalPlayer(player);
            return Result(world);
        }

        public IWorldEventResult Handle(IWorldEvent<SetWindowSlotPacket> message)
        {
            SetWindowSlotPacket packet = message.Event;
            IWorld world = message.World;
            IPlayer player = world.GetLocalPlayer();
            IPlayerInventory inventory = player.GetInventory();

            IItemStack itemStack = _itemStacks.Get(packet.Slot.BlockId)
                    .ChangeSize(packet.Slot.ItemCount);
            //.ChangeDurability(packet.Slot.ItemDamage);

            inventory = (IPlayerInventory)inventory.ChangeSlot(packet.SlotId, itemStack);
            player = player.ChangeInvetory(inventory);
            world = world.ReplaceLocalPlayer(player);
            return Result(world);
        }
    }
}
