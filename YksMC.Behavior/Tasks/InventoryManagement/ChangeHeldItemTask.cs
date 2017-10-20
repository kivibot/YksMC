using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Inventory;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Behavior.Tasks.InventoryManagement
{
    public class ChangeHeldItemTask : BehaviorTask<ChangeHeldItemCommand>
    {
        public override string Name => $"ChangeHeldItem({_command.HotbarSlot})";

        public ChangeHeldItemTask(ChangeHeldItemCommand command) 
            : base(command)
        {
        }

        public override IWorldEventResult OnStart(IWorld world)
        {
            IPlayer player = world.GetLocalPlayer();
            IPlayerInventory inventory = player.GetInventory();
            if(inventory.HeldItemIndex == _command.HotbarSlot)
            {
                Complete();
                return Result(world);
            }
            HeldItemChangePacket packet = new HeldItemChangePacket()
            {
                Slot = _command.HotbarSlot
            };
            inventory = inventory.ChangeHeldItem(_command.HotbarSlot);
            player = player.ChangeInvetory(inventory);
            world = world.ReplaceLocalPlayer(player);
            Complete();
            return Result(world, packet);
        }

        public override void OnTick(IWorld world, IGameTick tick)
        {
            return;
        }
    }
}
