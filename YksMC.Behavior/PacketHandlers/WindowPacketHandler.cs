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
using YksMC.Protocol.Packets.Play.Common;

namespace YksMC.Behavior.PacketHandlers
{
    public class WindowPacketHandler : WorldEventHandler, IWorldEventHandler<WindowItemsPacket>, IWorldEventHandler<SetWindowSlotPacket>,
        IWorldEventHandler<HeldItemChangePacket>, IWorldEventHandler<OpenWindowPacket>, IWorldEventHandler<CloseWindowPacket>,
        IWorldEventHandler<ConfirmTransactionPacket>
    {
        private readonly IGameObjectRegistry<IItemStack> _itemStackRegistry;
        private readonly IGameObjectRegistry<IWindow> _windowRegistry;

        public WindowPacketHandler(IGameObjectRegistry<IItemStack> itemStackRegistry, IGameObjectRegistry<IWindow> windowRegistry)
        {
            _itemStackRegistry = itemStackRegistry;
            _windowRegistry = windowRegistry;
        }

        public IWorldEventResult Handle(IWorldEvent<WindowItemsPacket> message)
        {
            WindowItemsPacket packet = message.Event;
            IWorld world = message.World;
            IWindow window = world.Windows[packet.WindowId];

            for (int index = 0; index < packet.Slots.Count; index++)
            {
                WindowSlotData slotData = packet.Slots.Values[index];
                IItemStack itemStack = _itemStackRegistry.Get<IItemStack>(slotData.BlockId)
                    .ChangeSize(slotData.ItemCount);
                //.ChangeDurability(slotData.ItemDamage);

                window = window.WithSlot(index, itemStack);
            }

            world = world.ReplaceWindow(window);
            return Result(world);
        }

        public IWorldEventResult Handle(IWorldEvent<SetWindowSlotPacket> message)
        {
            SetWindowSlotPacket packet = message.Event;
            IWorld world = message.World;
            packet.SlotId = packet.SlotId < 0 ? (short)0 : packet.SlotId;

            IWindow window = world.Windows[packet.WindowId];

            IItemStack itemStack = _itemStackRegistry.Get<IItemStack>(packet.Slot.BlockId)
                    .ChangeSize(packet.Slot.ItemCount);
            //.ChangeDurability(packet.Slot.ItemDamage);

            window = window.WithSlot(packet.SlotId, itemStack);
            world = world.ReplaceWindow(window);
            return Result(world);
        }

        public IWorldEventResult Handle(IWorldEvent<HeldItemChangePacket> message)
        {
            HeldItemChangePacket packet = message.Event;
            IWorld world = message.World;
            IPlayer player = world.GetLocalPlayer();
            IPlayerInventory inventory = player.GetInventory();

            inventory = inventory.ChangeHeldItem(packet.Slot);

            player = player.ChangeInvetory(inventory);
            world = world.ReplaceLocalPlayer(player);
            return Result(world);
        }

        public IWorldEventResult Handle(IWorldEvent<OpenWindowPacket> message)
        {
            OpenWindowPacket packet = message.Event;
            IWorld world = message.World;

            IWindow window = _windowRegistry.Get<IWindow>(packet.WindowType)
                .WithId(packet.WindowId)
                .WithTitle(packet.WindowTitle.Value)
                .WithUniqueSlots(packet.WindowSlotCount);

            world = world.ReplaceWindow(window);

            return Result(world);
        }

        public IWorldEventResult Handle(IWorldEvent<CloseWindowPacket> message)
        {
            CloseWindowPacket packet = message.Event;
            IWorld world = message.World;
            //TODO: ?
            if (packet.WindowId == 0 || packet.WindowId == 255)
            {
                return Result(world);
            }
            world = world.WithoutWindow(packet.WindowId);
            return Result(world);
        }

        public IWorldEventResult Handle(IWorldEvent<ConfirmTransactionPacket> message)
        {
            return Result(message.World, message.Event);
        }
    }
}
