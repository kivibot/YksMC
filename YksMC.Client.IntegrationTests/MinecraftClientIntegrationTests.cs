using Autofac;
using NUnit.Framework;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using YksMC.Behavior.Task;
using YksMC.Behavior.Urge;
using YksMC.Behavior.Urge.Scorers;
using YksMC.Bot.Core;
using YksMC.Bot.Login;
using YksMC.Bot.PacketHandlers;
using YksMC.Bot.Tasks;
using YksMC.Bot.TickHandlers;
using YksMC.Bot.UrgeConditions;
using YksMC.Client.Mapper;
using YksMC.Client.Worker;
using YksMC.Data.Json.Biome;
using YksMC.Data.Json.BlockType;
using YksMC.Data.Json.EntityType;
using YksMC.MinecraftModel.Biome;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.BlockType;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;
using YksMC.Protocol;
using YksMC.Protocol.Connection;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Nbt;
using YksMC.Protocol.Packets;
using YksMC.Protocol.Packets.Login;
using YksMC.Protocol.Serializing;
using Urge = YksMC.Behavior.Urge.Urge;

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
            builder.RegisterInstance(new MinecraftClientWorkerOptions() { IgnoreUnsupportedPackets = true });
            builder.RegisterInstance(new LoggerConfiguration().MinimumLevel.Verbose().WriteTo.Seq("http://localhost:5341").CreateLogger()).As<ILogger>();
            PacketTypeMapper typeMapper = new PacketTypeMapper();
            typeMapper.RegisterVanillaPackets();
            builder.RegisterInstance(typeMapper).AsImplementedInterfaces();

            builder.RegisterType<NbtReader>().AsImplementedInterfaces();

            builder.RegisterType<KeepAliveHandler>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<ChunkDataHandler>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<PlayerHandler>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<TimeUpdateHandler>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<BlockChangeHandler>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<EntityHandler>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<WorldEventHandlerWrapper>().AsSelf().SingleInstance();

            builder.RegisterType<PlayerMovementHandler>().AsSelf().SingleInstance();

            builder.RegisterType<JsonBiomeRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<JsonBlockTypeRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<JsonEntityTypeRepository>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<UrgeManager>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<LoginService>().AsImplementedInterfaces();
            builder.RegisterType<TaskLoop>().AsSelf();

            builder.RegisterType<AutofacBehaviorTaskManager>().AsImplementedInterfaces();

            builder.RegisterType<LoginTask>().AsImplementedInterfaces().Named<IBehaviorTask>("bt-Login");
            builder.RegisterType<LookAtNearestPlayerTask>().AsImplementedInterfaces().Named<IBehaviorTask>("bt-LookAtNearestPlayer");


            IBlock emptyBlock = new Block(new BlockType("air", false), new LightLevel(0), new LightLevel(0), new Biome("void"));
            IChunk emptyChunk = new Chunk(emptyBlock);
            IDimension dimension = new MinecraftModel.Dimension.Dimension(0, new DimensionType(true), emptyChunk);
            Dictionary<int, IDimension> dimensions = new Dictionary<int, IDimension>();
            dimensions[0] = dimension;
            IWorld world = new World(new Dictionary<Guid, IPlayer>(), null, dimensions, null);
            builder.RegisterInstance(world);

            _container = builder.Build();

            IUrgeManager urgeManager = _container.Resolve<IUrgeManager>();
            IMinecraftClient client = _container.Resolve<IMinecraftClient>();
            RegisterUrges(urgeManager, client);
        }

        private void RegisterUrges(IUrgeManager manager, IMinecraftClient client)
        {
            manager.AddUrge(new Urge("Login", "Login", new IUrgeScorer[] { new ConstantScorer(1) }, new IUrgeCondition[] { new ConnectionStateCondition(client, ConnectionState.None) }));
            manager.AddUrge(new Urge("LookAtNearestPlayer", "LookAtNearestPlayer", new IUrgeScorer[] { new ConstantScorer(0.1) }, new IUrgeCondition[] { new ConnectionStateCondition(client, ConnectionState.Play) }));
        }

        [TearDown]
        public void TearDown()
        {
            _container.Dispose();
        }

        [Test]
        public async Task ConnectAsync_WithRealServer_DoesNotCrash()
        {
            TaskLoop client = _container.Resolve<TaskLoop>();

            await client.LoopAsync();
        }

    }

    internal class AutofacBehaviorTaskManager : IBehaviorTaskManager
    {
        private readonly IComponentContext _componentContext;

        public AutofacBehaviorTaskManager(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public IBehaviorTask GetTask(string name)
        {
            IBehaviorTask task = _componentContext.ResolveNamed<IBehaviorTask>($"bt-{name}");
            if(task == null)
            {
                throw new ArgumentException($"unknown task: {name}");
            }
            return task;
        }
    }
}
