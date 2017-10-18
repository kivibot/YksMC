using Autofac;
using NUnit.Framework;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Urge;
using YksMC.Bot.Core;
using YksMC.Bot.Login;
using YksMC.Client.Mapper;
using YksMC.Client.Worker;
using YksMC.Data.Json.Biome;
using YksMC.Data.Json.BlockType;
using YksMC.Data.Json.EntityType;
using YksMC.EventBus.Bus;
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
using Urge = YksMC.Bot.Urge.Urge;
using YksMC.Behavior.PacketHandlers;
using YksMC.Behavior.TickHandlers;
using YksMC.Behavior.Tasks;
using YksMC.Behavior.UrgeScorers;
using YksMC.Behavior.UrgeConditions;
using System.Reflection.Metadata;
using YksMC.MinecraftModel.Entity;
using YksMC.Behavior.Misc;
using YksMC.MinecraftModel.Common;
using YksMC.Behavior.Tasks.Movement;
using YksMC.Behavior.Misc.Pathfinder;

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
            builder.RegisterType<EntitySpawnPacketHandler>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<EntityMovementPacketHandler>().AsImplementedInterfaces().AsSelf();

            builder.RegisterType<EventBus.Bus.EventBus>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AutofacHandlerContainer>().AsImplementedInterfaces();

            builder.RegisterType<PlayerGravityHandler>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<JsonBiomeRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<JsonBlockTypeRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<JsonEntityTypeRepository>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<UrgeManager>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<LoginService>().AsImplementedInterfaces();
            builder.RegisterType<TaskLoop>().AsSelf();

            builder.RegisterType<AutofacBehaviorTaskManager>().AsImplementedInterfaces();
            builder.RegisterType<PlayerCollisionDetectionService>().AsImplementedInterfaces();

            builder.RegisterType<LoginTask>().AsImplementedInterfaces().Named<IBehaviorTask>("bt-LoginCommand");
            builder.RegisterType<LookAtNearestPlayerTask>().AsImplementedInterfaces().Named<IBehaviorTask>("bt-LookAtNearestPlayerCommand");
            builder.RegisterType<RespawnTask>().AsImplementedInterfaces().Named<IBehaviorTask>("bt-RespawnCommand");
            builder.RegisterType<MoveLinearTask>().AsImplementedInterfaces().Named<IBehaviorTask>("bt-MoveLinearCommand");
            builder.RegisterType<MoveToLocationTask>().AsImplementedInterfaces().Named<IBehaviorTask>("bt-MoveToLocationCommand");
            builder.RegisterType<WalkToLocationTask>().AsImplementedInterfaces().Named<IBehaviorTask>("bt-WalkToLocationCommand");
            builder.RegisterType<JumpTask>().AsImplementedInterfaces().Named<IBehaviorTask>("bt-JumpCommand");

            builder.RegisterType<BehaviorTaskScheduler>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<PathFinder>().AsImplementedInterfaces();
            builder.RegisterType<Random>().SingleInstance();

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
            manager.AddUrge(new Urge(
                "Login",
                new LoginCommand(),
                new IUrgeScorer[] { new ConstantScorer(1) },
                new IUrgeCondition[] { new ConnectionStateCondition(client, ConnectionState.None) }
            ));
            manager.AddUrge(new Urge(
                "LookAtNearestPlayer",
                new LookAtNearestPlayerCommand(),
                new IUrgeScorer[] {
                    new ConstantScorer(0.1)
                },
                new IUrgeCondition[] {
                    new LoggedInCondition(),
                    new ConnectionStateCondition(client, ConnectionState.Play),
                    new AliveCondition()
                }
            ));
            manager.AddUrge(new Urge(
                "Respawn",
                new RespawnCommand(),
                new IUrgeScorer[] {
                    new ConstantScorer(1)
                },
                new IUrgeCondition[] {
                    new LoggedInCondition(),
                    new NotCondition(new AliveCondition())
                }
            ));
            manager.AddUrge(new Urge(
                "MoveToHardCoded",
                new WalkToLocationCommand(new BlockLocation(2683, 4, -806)),
                new IUrgeScorer[] {
                    new ConstantScorer(0.2)
                },
                new IUrgeCondition[] {
                    new LoggedInCondition(),
                    new AliveCondition()
                }
            ));
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

        public IBehaviorTask GetTask(object command)
        {
            return GetTaskInner((dynamic)command);
        }

        public IBehaviorTask GetTaskInner<T>(T command)
        {
            string commandName = typeof(T).Name;
            return _componentContext.ResolveNamed<IBehaviorTask>($"bt-{commandName}", TypedParameter.From(command));
        }
    }

    internal class AutofacHandlerContainer : IHandlerContainer
    {
        private readonly IComponentContext _componentContext;

        public AutofacHandlerContainer(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public IReadOnlyList<IEventHandler<TEvent, TResult>> GetHandlers<TEvent, TResult>()
        {
            return _componentContext.Resolve<IEnumerable<IEventHandler<TEvent, TResult>>>().ToList();
        }

        public void ReturnHandler<TEvent, TResult>(IEventHandler<TEvent, TResult> handler)
        {
            //TODO: implement
        }
    }
}
