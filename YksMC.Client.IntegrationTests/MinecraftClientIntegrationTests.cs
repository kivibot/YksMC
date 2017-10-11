using Autofac;
using NUnit.Framework;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.Handlers;
using YksMC.Client.EventBus;
using YksMC.Client.Injection;
using YksMC.Client.Mapper;
using YksMC.Client.Models.Status;
using YksMC.Client.Worker;
using YksMC.Data.Json.Biome;
using YksMC.Data.Json.BlockType;
using YksMC.Data.Json.EntityType;
using YksMC.MinecraftModel.Biome;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.BlockType;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Dimension;
using YksMC.Protocol;
using YksMC.Protocol.Connection;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Nbt;
using YksMC.Protocol.Serializing;

namespace YksMC.Client.IntegrationTests
{
    [TestFixture]
    public class MinecraftClientIntegrationTests
    {

        private IContainer _container;

        [SetUp]
        public void SetUp()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<MinecraftClient>().AsImplementedInterfaces().AsSelf().SingleInstance();
            builder.RegisterType<TcpClient>();
            builder.RegisterType<PacketSerializer>().AsImplementedInterfaces();
            builder.RegisterType<PacketDeserializer>().AsImplementedInterfaces();
            builder.RegisterType<PacketReader>().AsImplementedInterfaces();
            builder.RegisterType<PacketBuilder>().AsImplementedInterfaces();
            builder.RegisterType<StreamMinecraftConnection>().AsSelf();
            builder.RegisterType<MinecraftClientWorker>().AsImplementedInterfaces();
            builder.RegisterType<AutofacPacketHandlerFactory>().AsImplementedInterfaces();
            builder.RegisterInstance(new MinecraftClientWorkerOptions() { IgnoreUnsupportedPackets = true });
            builder.RegisterInstance(new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Seq("http://localhost:5341").CreateLogger()).As<ILogger>();
            PacketTypeMapper typeMapper = new PacketTypeMapper();
            typeMapper.RegisterVanillaPackets();
            builder.RegisterInstance(typeMapper).AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(AutofacOwnedWrapper<>)).AsImplementedInterfaces();
            builder.RegisterType<EventDispatcher>().AsImplementedInterfaces();
            builder.RegisterType<NbtReader>().AsImplementedInterfaces();

            builder.RegisterType<KeepAliveHandler>().AsImplementedInterfaces();
            builder.RegisterType<LoginHandler>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<ChunkDataHandler>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<PlayerHandler>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<WorldEventHandlerWrapper>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<JsonBiomeRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<JsonBlockTypeRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<JsonEntityTypeRepository>().AsImplementedInterfaces().SingleInstance();

            IBlock emptyBlock = new Block(new BlockType("air"), new LightLevel(0), new LightLevel(0), new Biome("void"));
            IChunk emptyChunk = new Chunk(emptyBlock);
            IDimension dimension = new MinecraftModel.Dimension.Dimension(new DimensionType(true), emptyChunk);
            builder.RegisterInstance(dimension);

            _container = builder.Build();
        }

        [TearDown]
        public void TearDown()
        {
            _container.Dispose();
        }

        [Test]
        public async Task ConnectAsync_WithRealServer_DoesNotCrash()
        {
            MinecraftClient client = _container.Resolve<MinecraftClient>();

            await client.ConnectAsync("localhost", 25565);

            client.SetState(ConnectionState.Login);
            client.SendHandshake(ConnectionState.Login);
            client.SendLoginStartPacket("testibotti");

            await Task.Delay(100000);
        }

    }
}
