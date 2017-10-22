using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.MinecraftModel.Inventory;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Behavior.Tasks.InventoryManagement
{
    public class ChangeHeldItemTask : BehaviorTask<ChangeHeldItemCommand>
    {
        public override string Name => $"ChangeHeldItem({_command.HotbarSlot})";

        public ChangeHeldItemTask(ChangeHeldItemCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler) 
            : base(command, minecraftClient, taskScheduler)
        {
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {
            IPlayer player = world.GetLocalPlayer();
            IPlayerInventory inventory = player.GetInventory();
            if(inventory.HeldItemIndex == _command.HotbarSlot)
            {
                return Success(world);
            }
            HeldItemChangePacket packet = new HeldItemChangePacket()
            {
                Slot = _command.HotbarSlot
            };
            _minecraftClient.SendPacket(packet);

            inventory = inventory.ChangeHeldItem(_command.HotbarSlot);
            player = player.ChangeInvetory(inventory);
            world = world.ReplaceLocalPlayer(player);
            return Success(world);
        }

        public override IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick)
        {
            return Result(world);
        }

        public override bool IsPossible(IWorld world)
        {
            IPlayer player = world.GetLocalPlayer();
            if(player == null)
            {
                return false;
            }
            return true;
        }
    }
}
