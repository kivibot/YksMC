using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Client;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Window;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks.InventoryManagement
{
    public class KeepBlockInventoryUpdatedTask : BehaviorTask<KeepBlockInventoryUpdatedCommand>
    {
        public override string Name => $"KeepBlockInventoryUpdated({_command.Location})";

        public KeepBlockInventoryUpdatedTask(KeepBlockInventoryUpdatedCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler)
            : base(command, minecraftClient, taskScheduler)
        {
            ChangePriority(BehaviorTaskPriority.High);
        }

        public override bool IsPossible(IWorld world)
        {
            IContainerBlock block = world.GetCurrentDimension().GetBlock<IContainerBlock>(_command.Location);
            if (block == null)
            {
                return false;
            }
            if (!world.Windows.ContainsWindow(_command.WindowId))
            {
                return false;
            }
            return true;
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {
            if (!IsPossible(world))
            {
                return Failure(world);
            }
            return UpdateInventory(world);
        }

        public override IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick)
        {
            if (!IsPossible(world))
            {
                return Success(world);
            }
            return UpdateInventory(world);
        }

        private IBehaviorTaskEventResult UpdateInventory(IWorld world)
        {
            IContainerBlock block = world.GetCurrentDimension().GetBlock<IContainerBlock>(_command.Location);
            IWindow window = world.Windows[_command.WindowId];
            for (int i = 0; i < block.GetInventory().SlotCount && i < window.UniqueSlotCount; i++)
            {
                block = block.WithInventorySlot(i, window.Slots[i]);
            }

            IChunk chunk = world.GetCurrentDimension().GetChunk(new ChunkCoordinate(_command.Location))
                .ChangeBlock(_command.Location, block);
            world = world.ChangeCurrentDimension(dimension => dimension.ReplaceChunk(new ChunkCoordinate(_command.Location), chunk));

            return Result(world);
        }
    }
}
