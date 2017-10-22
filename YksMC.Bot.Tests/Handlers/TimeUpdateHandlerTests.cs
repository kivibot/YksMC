using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Behavior.PacketHandlers;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Biome;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.Window;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Bot.Tests.Handlers
{
    [TestFixture]
    public class TimeUpdateHandlerTests
    {

        [Test]
        public void ApplyTimeUpdate_PositiveValues_CorrectAgeTimeSet()
        {
            TimeUpdateHandler handler = new TimeUpdateHandler();
            TimeUpdatePacket packet = new TimeUpdatePacket()
            {
                WorldAge = 12341337,
                TimeOfDay = 25000
            };
            IWorld world = GetWorld();

            IWorldEventResult result = handler.Handle(new WorldEvent<TimeUpdatePacket>(world, packet));

            Assert.AreEqual(12341337, result.World.GetCurrentDimension().AgeAndTime.AgeTicks);
            Assert.AreEqual(1000, result.World.GetCurrentDimension().AgeAndTime.TimeTicks);
        }

        private IWorld GetWorld()
        {
            IBlock emptyBlock = new Block("air", false, false, 0, HarvestTier.Hand, BlockMaterial.Normal, false, true);
            IChunk emptyChunk = new Chunk(emptyBlock);
            IDimension dimension = new MinecraftModel.Dimension.Dimension(0, new DimensionType(true), emptyChunk);
            Dictionary<int, IDimension> dimensions = new Dictionary<int, IDimension>();
            dimensions[0] = dimension;
            IWindow inventoryWindow = new Window("inventory").WithUniqueSlots(10);
            IWindowCollection windowCollection = new WindowCollection()
                .WithWindow(inventoryWindow);
            IWorld world = new World(new Dictionary<Guid, IPlayer>(), null, dimensions, dimension, windowCollection);
            return world;
        }

    }
}
