using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.PacketHandlers;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Biome;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.BlockType;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Player;
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

            IWorldEventResult result = handler.ApplyEvent(packet, world);

            Assert.AreEqual(12341337, result.World.GetCurrentDimension().AgeAndTime.AgeTicks);
            Assert.AreEqual(1000, result.World.GetCurrentDimension().AgeAndTime.TimeTicks);
        }

        private IWorld GetWorld()
        {
            IBlock emptyBlock = new Block(new BlockType("air"), new LightLevel(0), new LightLevel(0), new Biome("void"));
            IChunk emptyChunk = new Chunk(emptyBlock);
            IDimension dimension = new MinecraftModel.Dimension.Dimension(0, new DimensionType(true), emptyChunk);
            Dictionary<int, IDimension> dimensions = new Dictionary<int, IDimension>();
            dimensions[0] = dimension;
            IWorld world = new World(new Dictionary<IPlayerId, IPlayer>(), null, dimensions, dimension);
            return world;
        }

    }
}
