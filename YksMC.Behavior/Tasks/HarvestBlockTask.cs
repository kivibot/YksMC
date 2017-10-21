using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YksMC.Behavior.Tasks.InventoryManagement;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.BlockType;
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
        private readonly IBehaviorTaskScheduler _taskScheduler;

        public override string Name => $"HarvestBlock({_command.Location})";

        public HarvestBlockTask(HarvestBlockCommand command, IBehaviorTaskScheduler taskScheduler)
            : base(command)
        {
            _taskScheduler = taskScheduler;
        }

        public override IWorldEventResult OnStart(IWorld world)
        {
            IBlock block = world.GetCurrentDimension().GetBlock(_command.Location);
            if (!block.Type.IsDiggable)
            {
                Fail();
                return Result(world);
            }
            IEntity entity = world.GetPlayerEntity();
            IPlayer player = world.GetLocalPlayer();
            IPlayerInventory inventory = player.GetInventory();

            HarvestBlockAsync(block, inventory);

            return Result(world);
        }

        private async void HarvestBlockAsync(IBlock block, IPlayerInventory inventory)
        {
            if (!await SelectBestToolInHotbarAsync(block.Type, inventory))
            {
                Fail();
                return;
            }
            if ((await _taskScheduler.RunTaskAsync(new BreakBlockCommand(_command.Location))).IsFailed)
            {
                Fail();
                return;
            }
            Complete();
        }

        private async Task<bool> SelectBestToolInHotbarAsync(IBlockType blockType, IPlayerInventory inventory)
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
                if (!tool.CanHarvest(blockType))
                {
                    continue;
                }
                if (bestTool != null && bestTool.GetBreakingSpeed(blockType) >= tool.GetBreakingSpeed(blockType))
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
            if ((await _taskScheduler.RunTaskAsync(new ChangeHeldItemCommand(bestToolSlot))).IsFailed)
            {
                Fail();
                return false;
            }
            return true;
        }

        public override void OnTick(IWorld world, IGameTick tick)
        {
            return;
        }
    }
}
