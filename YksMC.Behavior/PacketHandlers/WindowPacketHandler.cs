using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.ItemType;
using YksMC.MinecraftModel.Window;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Behavior.PacketHandlers
{
    public class WindowPacketHandler : WorldEventHandler, IWorldEventHandler<WindowItemsPacket>
    {
        private readonly IItemTypeRepository _itemTypeRepository;

        public WindowPacketHandler(IItemTypeRepository itemTypeRepository)
        {
            _itemTypeRepository = itemTypeRepository;
        }

        public IWorldEventResult Handle(IWorldEvent<WindowItemsPacket> message)
        {
            WindowItemsPacket packet = message.Event;
            IWindow window = message.World.Windows[packet.WindowId];
            for (int index = 0; index < packet.Slots.Count; index++)
            {
                WindowSlotData slotData = packet.Slots.Values[index];
                IItemType itemType = _itemTypeRepository.Get(slotData.BlockId);
                IWindowSlot slot = new WindowSlot(itemType, slotData.ItemCount, slotData.ItemDamage);
                window = window.ReplaceSlot(slot);
            }
            return Result(message.World.ReplaceWindow(window));
        }
    }
}
