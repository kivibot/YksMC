using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YksMC.Behavior.Tasks.InventoryManagement;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Inventory;
using YksMC.MinecraftModel.ItemStack;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks
{
    public class HarvestBlockTask : BehaviorTask<HarvestBlockCommand>
    {
        public override string Name => $"HarvestBlock({_command.Location})";

        public HarvestBlockTask(HarvestBlockCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler) 
            : base(command, minecraftClient, taskScheduler)
        {
        }

        public override bool IsPossible(IWorld world)
        {
            IBlock block = world.GetCurrentDimension().GetBlock<IBlock>(_command.Location);
            if (!block.IsDiggable)
            {                
                return false;
            }
            return true;
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {
            return Result(world);
        }

        public override async Task<bool?> OnStartAsync(IWorld world)
        {
            IBlock block = world.GetCurrentDimension().GetBlock<IBlock>(_command.Location);
            IEntity entity = world.GetPlayerEntity();
            IPlayer player = world.GetLocalPlayer();
            IPlayerInventory inventory = player.GetInventory();
            return await HarvestBlockAsync(block, inventory);
        }

        private async Task<bool> HarvestBlockAsync(IBlock block, IPlayerInventory inventory)
        {
            if (!await SelectBestToolInHotbarAsync(block, inventory))
            {
                return false;
            }
            if (!(await _taskScheduler.RunCommandAsync(new BreakBlockCommand(_command.Location))))
            {
                return false;
            }
            return true;
        }

        private async Task<bool> SelectBestToolInHotbarAsync(IBlock block, IPlayerInventory inventory)
        {
            int bestToolSlot = -1;
            IHarvestingTool bestTool = null;
            for (int slot = 0; slot < 9; slot++)
            {
                IHarvestingTool tool = inventory.GetHotbarSlot<IHarvestingTool>(slot);
                if (tool == null)
                {
                    continue;
                }
                if (!tool.CanHarvest(block))
                {
                    continue;
                }
                if (bestTool != null && bestTool.GetBreakingSpeed(block) >= tool.GetBreakingSpeed(block))
                {
                    continue;
                }
                bestTool = tool;
                bestToolSlot = slot;
            }
            if (bestTool == null)
            {
                return !_command.FailWhenNoRightToolAvailable;
            }
            if (!(await _taskScheduler.RunCommandAsync(new ChangeHeldItemCommand(bestToolSlot))))
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
