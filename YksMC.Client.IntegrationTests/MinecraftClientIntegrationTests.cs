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
using YksMC.Data.Json.EntityType;
using YksMC.EventBus.Bus;
using YksMC.MinecraftModel.Biome;
using YksMC.MinecraftModel.Block;
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
using YksMC.MinecraftModel.Window;
using YksMC.Data.Json.Window;
using YksMC.Data.Json.ItemType;
using YksMC.Bot.GameObjectRegistry;
using YksMC.MinecraftModel.Inventory;
using YksMC.MinecraftModel.ItemStack;
using YksMC.Behavior.Tasks.InventoryManagement;
using YksMC.Behavior.Tasks.Building;

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
            builder.RegisterType<WindowPacketHandler>().AsImplementedInterfaces().AsSelf();

            builder.RegisterType<EventBus.Bus.EventBus>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AutofacHandlerContainer>().AsImplementedInterfaces();

            builder.RegisterType<PlayerGravityHandler>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<JsonBiomeRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<JsonEntityTypeRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<JsonWindowTypeRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<JsonItemTypeRepository>().AsImplementedInterfaces().SingleInstance();

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
            builder.RegisterType<LookAtTask>().AsImplementedInterfaces().Named<IBehaviorTask>("bt-LookAtCommand");
            builder.RegisterType<BreakBlockTask>().AsImplementedInterfaces().Named<IBehaviorTask>("bt-BreakBlockCommand");
            builder.RegisterType<ChangeHeldItemTask>().AsImplementedInterfaces().Named<IBehaviorTask>("bt-ChangeHeldItemCommand");
            builder.RegisterType<HarvestBlockTask>().AsImplementedInterfaces().Named<IBehaviorTask>("bt-HarvestBlockCommand");
            builder.RegisterType<PlaceHeldBlockTask>().AsImplementedInterfaces().Named<IBehaviorTask>("bt-PlaceHeldBlockCommand");
            builder.RegisterType<OpenBlockWindowTask>().AsImplementedInterfaces().Named<IBehaviorTask>("bt-OpenBlockWindowCommand");

            builder.RegisterType<BehaviorTaskScheduler>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<PathFinder>().AsImplementedInterfaces();
            builder.RegisterType<Random>().SingleInstance();

            IGameObjectRegistry<IInventory> inventoryRegistry = new GameObjectRegistry<IInventory>();
            inventoryRegistry.Register(new PlayerInventory(), "minecraft:player");
            builder.RegisterInstance(inventoryRegistry).As<IGameObjectRegistry<IInventory>>();

            IGameObjectRegistry<IItemStack> itemRegistry = new GameObjectRegistry<IItemStack>();
            RegisterVanillaItems(itemRegistry);
            builder.RegisterInstance(itemRegistry).As<IGameObjectRegistry<IItemStack>>();

            IGameObjectRegistry<IBlock> blockRegistry = new GameObjectRegistry<IBlock>();
            RegisterVanillaBlocks(blockRegistry);
            builder.RegisterInstance(blockRegistry).As<IGameObjectRegistry<IBlock>>();

            IBlock emptyBlock = blockRegistry.Get<IBlock>("minecraft:air")
                .WithBiome(new Biome("void"));
            IChunk emptyChunk = new Chunk(emptyBlock);
            IDimension dimension = new MinecraftModel.Dimension.Dimension(0, new DimensionType(true), emptyChunk);
            Dictionary<int, IDimension> dimensions = new Dictionary<int, IDimension>();
            dimensions[0] = dimension;
            IWindow inventoryWindow = new Window(0, "inventory", new List<IWindowSlot>());
            IWindowCollection windowCollection = new WindowCollection()
                .ReplaceWindow(inventoryWindow);
            IWorld world = new World(new Dictionary<Guid, IPlayer>(), null, dimensions, null, windowCollection);
            builder.RegisterInstance(world);

            _container = builder.Build();

            IUrgeManager urgeManager = _container.Resolve<IUrgeManager>();
            IMinecraftClient client = _container.Resolve<IMinecraftClient>();
            RegisterUrges(urgeManager, client);
        }

        private void RegisterUrges(IUrgeManager manager, IMinecraftClient client)
        {
            Random random = new Random();

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
                    new ConstantScorer(0.2)
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
                new WalkToLocationCommand(new BlockLocation(2678, 4, -796)),
                new IUrgeScorer[] {
                    new DistanceScorer(new EntityLocation(2678.5, 4, -795.5), 1)
                },
                new IUrgeCondition[] {
                    new LoggedInCondition(),
                    new AliveCondition()
                }
            ));
            manager.AddUrge(new Urge(
                "BreakBlockHardCoded",
                new HarvestBlockCommand(new BlockLocation(2676, 4, -796), false),
                new IUrgeScorer[] {
                    new ConstantScorer(0.5)
                },
                new IUrgeCondition[] {
                    new LoggedInCondition(),
                    new AliveCondition(),
                    new SolidBlockCondition(new BlockLocation(2676, 4, -796))
                }
            ));
            manager.AddUrge(new Urge(
                "PlaceBlockHardCoded",
                new PlaceHeldBlockCommand(new BlockLocation(2676, 4, -796)),
                new IUrgeScorer[] {
                    new ConstantScorer(0.5)
                },
                new IUrgeCondition[] {
                    new LoggedInCondition(),
                    new AliveCondition(),
                    new NotCondition(new SolidBlockCondition(new BlockLocation(2676, 4, -796)))
                }
            ));
            manager.AddUrge(new Urge(
                "OpenBlockWindowHardCoded",
                new OpenBlockWindowCommand(new BlockLocation(2676, 4, -796)),
                new IUrgeScorer[] {
                    new ConstantScorer(-0.6)
                },
                new IUrgeCondition[] {
                    new LoggedInCondition(),
                    new AliveCondition(),
                    new SolidBlockCondition(new BlockLocation(2676, 4, -796))
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

#region Game objects

        private void RegisterVanillaBlocks(IGameObjectRegistry<IBlock> blockRegistry)
        {
            blockRegistry.Register(new Block("minecraft:air", false, false, 0, HarvestTier.Hand, BlockMaterial.Normal, false, true), 0, "minecraft:air");
            blockRegistry.Register(new Block("minecraft:stone", true, true, 1.5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 1, "minecraft:stone");
            blockRegistry.Register(new Block("minecraft:grass", true, true, 0.6, HarvestTier.Hand, BlockMaterial.Dirt, false, false), 2, "minecraft:grass");
            blockRegistry.Register(new Block("minecraft:dirt", true, true, 0.5, HarvestTier.Hand, BlockMaterial.Dirt, false, false), 3, "minecraft:dirt");
            blockRegistry.Register(new Block("minecraft:cobblestone", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 4, "minecraft:cobblestone");
            blockRegistry.Register(new Block("minecraft:planks", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 5, "minecraft:planks");
            blockRegistry.Register(new Block("minecraft:sapling", false, true, 0, HarvestTier.Hand, BlockMaterial.Plant, false, false), 6, "minecraft:sapling");
            blockRegistry.Register(new Block("minecraft:bedrock", true, false, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 7, "minecraft:bedrock");
            blockRegistry.Register(new Block("minecraft:flowing_water", false, false, 100, HarvestTier.Hand, BlockMaterial.Normal, false, false), 8, "minecraft:flowing_water");
            blockRegistry.Register(new Block("minecraft:water", false, false, 100, HarvestTier.Hand, BlockMaterial.Normal, false, false), 9, "minecraft:water");
            blockRegistry.Register(new Block("minecraft:flowing_lava", false, false, 100, HarvestTier.Hand, BlockMaterial.Normal, true, false), 10, "minecraft:flowing_lava");
            blockRegistry.Register(new Block("minecraft:lava", false, false, 100, HarvestTier.Hand, BlockMaterial.Normal, true, false), 11, "minecraft:lava");
            blockRegistry.Register(new Block("minecraft:sand", true, true, 0.5, HarvestTier.Hand, BlockMaterial.Dirt, false, false), 12, "minecraft:sand");
            blockRegistry.Register(new Block("minecraft:gravel", true, true, 0.6, HarvestTier.Hand, BlockMaterial.Dirt, false, false), 13, "minecraft:gravel");
            blockRegistry.Register(new Block("minecraft:gold_ore", true, true, 3, HarvestTier.Iron, BlockMaterial.Rock, false, false), 14, "minecraft:gold_ore");
            blockRegistry.Register(new Block("minecraft:iron_ore", true, true, 3, HarvestTier.Stone, BlockMaterial.Rock, false, false), 15, "minecraft:iron_ore");
            blockRegistry.Register(new Block("minecraft:coal_ore", true, true, 3, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 16, "minecraft:coal_ore");
            blockRegistry.Register(new Block("minecraft:log", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 17, "minecraft:log");
            blockRegistry.Register(new Block("minecraft:leaves", true, true, 0.2, HarvestTier.Hand, BlockMaterial.Plant, false, false), 18, "minecraft:leaves");
            blockRegistry.Register(new Block("minecraft:sponge", true, true, 0.6, HarvestTier.Hand, BlockMaterial.Normal, false, false), 19, "minecraft:sponge");
            blockRegistry.Register(new Block("minecraft:glass", true, true, 0.3, HarvestTier.Hand, BlockMaterial.Normal, false, false), 20, "minecraft:glass");
            blockRegistry.Register(new Block("minecraft:lapis_ore", true, true, 3, HarvestTier.Stone, BlockMaterial.Rock, false, false), 21, "minecraft:lapis_ore");
            blockRegistry.Register(new Block("minecraft:lapis_block", true, true, 3, HarvestTier.Stone, BlockMaterial.Rock, false, false), 22, "minecraft:lapis_block");
            blockRegistry.Register(new Block("minecraft:dispenser", true, true, 3.5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 23, "minecraft:dispenser");
            blockRegistry.Register(new Block("minecraft:sandstone", true, true, 0.8, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 24, "minecraft:sandstone");
            blockRegistry.Register(new Block("minecraft:noteblock", true, true, 0.8, HarvestTier.Hand, BlockMaterial.Wood, false, false), 25, "minecraft:noteblock");
            blockRegistry.Register(new Block("minecraft:bed", true, true, 0.2, HarvestTier.Hand, BlockMaterial.Normal, false, false), 26, "minecraft:bed");
            blockRegistry.Register(new Block("minecraft:golden_rail", false, true, 0.7, HarvestTier.Hand, BlockMaterial.Rock, false, false), 27, "minecraft:golden_rail");
            blockRegistry.Register(new Block("minecraft:detector_rail", false, true, 0.7, HarvestTier.Hand, BlockMaterial.Rock, false, false), 28, "minecraft:detector_rail");
            blockRegistry.Register(new Block("minecraft:sticky_piston", true, true, 0.5, HarvestTier.Hand, BlockMaterial.Normal, false, false), 29, "minecraft:sticky_piston");
            blockRegistry.Register(new Block("minecraft:web", false, true, 4, HarvestTier.Wooden, BlockMaterial.Web, false, false), 30, "minecraft:web");
            blockRegistry.Register(new Block("minecraft:tallgrass", false, true, 0, HarvestTier.Hand, BlockMaterial.Dirt, false, false), 31, "minecraft:tallgrass");
            blockRegistry.Register(new Block("minecraft:deadbush", false, true, 0, HarvestTier.Hand, BlockMaterial.Plant, false, false), 32, "minecraft:deadbush");
            blockRegistry.Register(new Block("minecraft:piston", true, true, 0.5, HarvestTier.Hand, BlockMaterial.Normal, false, false), 33, "minecraft:piston");
            blockRegistry.Register(new Block("minecraft:piston_head", true, true, 0.5, HarvestTier.Hand, BlockMaterial.Normal, false, false), 34, "minecraft:piston_head");
            blockRegistry.Register(new Block("minecraft:wool", true, true, 0.8, HarvestTier.Hand, BlockMaterial.Wool, false, false), 35, "minecraft:wool");
            blockRegistry.Register(new Block("minecraft:piston_extension", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 36, "minecraft:piston_extension");
            blockRegistry.Register(new Block("minecraft:yellow_flower", false, true, 0, HarvestTier.Hand, BlockMaterial.Plant, false, false), 37, "minecraft:yellow_flower");
            blockRegistry.Register(new Block("minecraft:red_flower", false, true, 0, HarvestTier.Hand, BlockMaterial.Plant, false, false), 38, "minecraft:red_flower");
            blockRegistry.Register(new Block("minecraft:brown_mushroom", true, true, 0, HarvestTier.Hand, BlockMaterial.Plant, false, false), 39, "minecraft:brown_mushroom");
            blockRegistry.Register(new Block("minecraft:red_mushroom", true, true, 0, HarvestTier.Hand, BlockMaterial.Plant, false, false), 40, "minecraft:red_mushroom");
            blockRegistry.Register(new Block("minecraft:gold_block", true, true, 3, HarvestTier.Iron, BlockMaterial.Rock, false, false), 41, "minecraft:gold_block");
            blockRegistry.Register(new Block("minecraft:iron_block", true, true, 5, HarvestTier.Stone, BlockMaterial.Rock, false, false), 42, "minecraft:iron_block");
            blockRegistry.Register(new Block("minecraft:double_stone_slab", true, true, 0, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 43, "minecraft:double_stone_slab");
            blockRegistry.Register(new Block("minecraft:stone_slab", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 44, "minecraft:stone_slab");
            blockRegistry.Register(new Block("minecraft:brick_block", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 45, "minecraft:brick_block");
            blockRegistry.Register(new Block("minecraft:tnt", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 46, "minecraft:tnt");
            blockRegistry.Register(new Block("minecraft:bookshelf", true, true, 1.5, HarvestTier.Hand, BlockMaterial.Wood, false, false), 47, "minecraft:bookshelf");
            blockRegistry.Register(new Block("minecraft:mossy_cobblestone", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 48, "minecraft:mossy_cobblestone");
            blockRegistry.Register(new Block("minecraft:obsidian", true, true, 50, HarvestTier.Diamond, BlockMaterial.Rock, false, false), 49, "minecraft:obsidian");
            blockRegistry.Register(new Block("minecraft:torch", false, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 50, "minecraft:torch");
            blockRegistry.Register(new Block("minecraft:fire", false, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 51, "minecraft:fire");
            blockRegistry.Register(new Block("minecraft:mob_spawner", true, true, 5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 52, "minecraft:mob_spawner");
            blockRegistry.Register(new Block("minecraft:oak_stairs", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 53, "minecraft:oak_stairs");
            blockRegistry.Register(new Block("minecraft:chest", true, true, 2.5, HarvestTier.Hand, BlockMaterial.Wood, false, false), 54, "minecraft:chest");
            blockRegistry.Register(new Block("minecraft:redstone_wire", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 55, "minecraft:redstone_wire");
            blockRegistry.Register(new Block("minecraft:diamond_ore", true, true, 3, HarvestTier.Iron, BlockMaterial.Rock, false, false), 56, "minecraft:diamond_ore");
            blockRegistry.Register(new Block("minecraft:diamond_block", true, true, 5, HarvestTier.Iron, BlockMaterial.Rock, false, false), 57, "minecraft:diamond_block");
            blockRegistry.Register(new Block("minecraft:crafting_table", true, true, 2.5, HarvestTier.Hand, BlockMaterial.Wood, false, false), 58, "minecraft:crafting_table");
            blockRegistry.Register(new Block("minecraft:wheat", false, true, 0, HarvestTier.Hand, BlockMaterial.Plant, false, false), 59, "minecraft:wheat");
            blockRegistry.Register(new Block("minecraft:farmland", true, true, 0.6, HarvestTier.Hand, BlockMaterial.Dirt, false, false), 60, "minecraft:farmland");
            blockRegistry.Register(new Block("minecraft:furnace", true, true, 3.5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 61, "minecraft:furnace");
            blockRegistry.Register(new Block("minecraft:lit_furnace", true, true, 3.5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 62, "minecraft:lit_furnace");
            blockRegistry.Register(new Block("minecraft:standing_sign", false, true, 1, HarvestTier.Hand, BlockMaterial.Wood, false, false), 63, "minecraft:standing_sign");
            blockRegistry.Register(new Block("minecraft:wooden_door", true, true, 3, HarvestTier.Hand, BlockMaterial.Wood, false, false), 64, "minecraft:wooden_door");
            blockRegistry.Register(new Block("minecraft:ladder", true, true, 0.4, HarvestTier.Hand, BlockMaterial.Normal, false, false), 65, "minecraft:ladder");
            blockRegistry.Register(new Block("minecraft:rail", false, true, 0.7, HarvestTier.Hand, BlockMaterial.Rock, false, false), 66, "minecraft:rail");
            blockRegistry.Register(new Block("minecraft:stone_stairs", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 67, "minecraft:stone_stairs");
            blockRegistry.Register(new Block("minecraft:wall_sign", false, true, 1, HarvestTier.Hand, BlockMaterial.Wood, false, false), 68, "minecraft:wall_sign");
            blockRegistry.Register(new Block("minecraft:lever", false, true, 0.5, HarvestTier.Hand, BlockMaterial.Normal, false, false), 69, "minecraft:lever");
            blockRegistry.Register(new Block("minecraft:stone_pressure_plate", false, true, 0.5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 70, "minecraft:stone_pressure_plate");
            blockRegistry.Register(new Block("minecraft:iron_door", true, true, 5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 71, "minecraft:iron_door");
            blockRegistry.Register(new Block("minecraft:wooden_pressure_plate", false, true, 0.5, HarvestTier.Hand, BlockMaterial.Wood, false, false), 72, "minecraft:wooden_pressure_plate");
            blockRegistry.Register(new Block("minecraft:redstone_ore", true, true, 3, HarvestTier.Iron, BlockMaterial.Rock, false, false), 73, "minecraft:redstone_ore");
            blockRegistry.Register(new Block("minecraft:lit_redstone_ore", true, true, 3, HarvestTier.Iron, BlockMaterial.Rock, false, false), 74, "minecraft:lit_redstone_ore");
            blockRegistry.Register(new Block("minecraft:unlit_redstone_torch", false, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 75, "minecraft:unlit_redstone_torch");
            blockRegistry.Register(new Block("minecraft:redstone_torch", false, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 76, "minecraft:redstone_torch");
            blockRegistry.Register(new Block("minecraft:stone_button", false, true, 0.5, HarvestTier.Hand, BlockMaterial.Normal, false, false), 77, "minecraft:stone_button");
            blockRegistry.Register(new Block("minecraft:snow_layer", true, true, 0.2, HarvestTier.Wooden, BlockMaterial.Dirt, false, false), 78, "minecraft:snow_layer");
            blockRegistry.Register(new Block("minecraft:ice", true, true, 0.5, HarvestTier.Hand, BlockMaterial.Rock, false, false), 79, "minecraft:ice");
            blockRegistry.Register(new Block("minecraft:snow", true, true, 0.2, HarvestTier.Wooden, BlockMaterial.Dirt, false, false), 80, "minecraft:snow");
            blockRegistry.Register(new Block("minecraft:cactus", true, true, 0.4, HarvestTier.Hand, BlockMaterial.Plant, true, false), 81, "minecraft:cactus");
            blockRegistry.Register(new Block("minecraft:clay", true, true, 0.6, HarvestTier.Hand, BlockMaterial.Dirt, false, false), 82, "minecraft:clay");
            blockRegistry.Register(new Block("minecraft:reeds", false, true, 0, HarvestTier.Hand, BlockMaterial.Plant, false, false), 83, "minecraft:reeds");
            blockRegistry.Register(new Block("minecraft:jukebox", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 84, "minecraft:jukebox");
            blockRegistry.Register(new Block("minecraft:fence", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 85, "minecraft:fence");
            blockRegistry.Register(new Block("minecraft:pumpkin", true, true, 1, HarvestTier.Hand, BlockMaterial.Plant, false, false), 86, "minecraft:pumpkin");
            blockRegistry.Register(new Block("minecraft:netherrack", true, true, 0.4, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 87, "minecraft:netherrack");
            blockRegistry.Register(new Block("minecraft:soul_sand", true, true, 0.5, HarvestTier.Hand, BlockMaterial.Dirt, false, false), 88, "minecraft:soul_sand");
            blockRegistry.Register(new Block("minecraft:glowstone", true, true, 0.3, HarvestTier.Hand, BlockMaterial.Normal, false, false), 89, "minecraft:glowstone");
            blockRegistry.Register(new Block("minecraft:portal", false, false, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 90, "minecraft:portal");
            blockRegistry.Register(new Block("minecraft:lit_pumpkin", true, true, 1, HarvestTier.Hand, BlockMaterial.Plant, false, false), 91, "minecraft:lit_pumpkin");
            blockRegistry.Register(new Block("minecraft:cake", false, true, 0.5, HarvestTier.Hand, BlockMaterial.Normal, false, false), 92, "minecraft:cake");
            blockRegistry.Register(new Block("minecraft:unpowered_repeater", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 93, "minecraft:unpowered_repeater");
            blockRegistry.Register(new Block("minecraft:powered_repeater", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 94, "minecraft:powered_repeater");
            blockRegistry.Register(new Block("minecraft:stained_glass", true, true, 0.3, HarvestTier.Hand, BlockMaterial.Normal, false, false), 95, "minecraft:stained_glass");
            blockRegistry.Register(new Block("minecraft:trapdoor", true, true, 3, HarvestTier.Hand, BlockMaterial.Wood, false, false), 96, "minecraft:trapdoor");
            blockRegistry.Register(new Block("minecraft:monster_egg", true, true, 0.75, HarvestTier.Hand, BlockMaterial.Normal, false, false), 97, "minecraft:monster_egg");
            blockRegistry.Register(new Block("minecraft:stonebrick", true, true, 1.5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 98, "minecraft:stonebrick");
            blockRegistry.Register(new Block("minecraft:brown_mushroom_block", true, true, 0, HarvestTier.Hand, BlockMaterial.Wood, false, false), 99, "minecraft:brown_mushroom_block");
            blockRegistry.Register(new Block("minecraft:red_mushroom_block", true, true, 0, HarvestTier.Hand, BlockMaterial.Wood, false, false), 100, "minecraft:red_mushroom_block");
            blockRegistry.Register(new Block("minecraft:iron_bars", true, true, 5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 101, "minecraft:iron_bars");
            blockRegistry.Register(new Block("minecraft:glass_pane", true, true, 0.3, HarvestTier.Hand, BlockMaterial.Normal, false, false), 102, "minecraft:glass_pane");
            blockRegistry.Register(new Block("minecraft:melon_block", true, true, 1, HarvestTier.Hand, BlockMaterial.Plant, false, false), 103, "minecraft:melon_block");
            blockRegistry.Register(new Block("minecraft:pumpkin_stem", false, true, 0, HarvestTier.Hand, BlockMaterial.Plant, false, false), 104, "minecraft:pumpkin_stem");
            blockRegistry.Register(new Block("minecraft:melon_stem", false, true, 0, HarvestTier.Hand, BlockMaterial.Plant, false, false), 105, "minecraft:melon_stem");
            blockRegistry.Register(new Block("minecraft:vine", false, true, 0.2, HarvestTier.Hand, BlockMaterial.Plant, false, false), 106, "minecraft:vine");
            blockRegistry.Register(new Block("minecraft:fence_gate", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 107, "minecraft:fence_gate");
            blockRegistry.Register(new Block("minecraft:brick_stairs", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 108, "minecraft:brick_stairs");
            blockRegistry.Register(new Block("minecraft:stone_brick_stairs", true, true, 1.5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 109, "minecraft:stone_brick_stairs");
            blockRegistry.Register(new Block("minecraft:mycelium", true, true, 0.6, HarvestTier.Hand, BlockMaterial.Dirt, false, false), 110, "minecraft:mycelium");
            blockRegistry.Register(new Block("minecraft:waterlily", true, true, 0, HarvestTier.Hand, BlockMaterial.Plant, false, false), 111, "minecraft:waterlily");
            blockRegistry.Register(new Block("minecraft:nether_brick", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 112, "minecraft:nether_brick");
            blockRegistry.Register(new Block("minecraft:nether_brick_fence", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 113, "minecraft:nether_brick_fence");
            blockRegistry.Register(new Block("minecraft:nether_brick_stairs", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 114, "minecraft:nether_brick_stairs");
            blockRegistry.Register(new Block("minecraft:nether_wart", false, true, 0, HarvestTier.Hand, BlockMaterial.Plant, false, false), 115, "minecraft:nether_wart");
            blockRegistry.Register(new Block("minecraft:enchanting_table", true, true, 5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 116, "minecraft:enchanting_table");
            blockRegistry.Register(new Block("minecraft:brewing_stand", true, true, 0.5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 117, "minecraft:brewing_stand");
            blockRegistry.Register(new Block("minecraft:cauldron", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 118, "minecraft:cauldron");
            blockRegistry.Register(new Block("minecraft:end_portal", false, false, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 119, "minecraft:end_portal");
            blockRegistry.Register(new Block("minecraft:end_portal_frame", true, false, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 120, "minecraft:end_portal_frame");
            blockRegistry.Register(new Block("minecraft:end_stone", true, true, 3, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 121, "minecraft:end_stone");
            blockRegistry.Register(new Block("minecraft:dragon_egg", true, true, 3, HarvestTier.Hand, BlockMaterial.Normal, false, false), 122, "minecraft:dragon_egg");
            blockRegistry.Register(new Block("minecraft:redstone_lamp", true, true, 0.3, HarvestTier.Hand, BlockMaterial.Normal, false, false), 123, "minecraft:redstone_lamp");
            blockRegistry.Register(new Block("minecraft:lit_redstone_lamp", true, true, 0.3, HarvestTier.Hand, BlockMaterial.Normal, false, false), 124, "minecraft:lit_redstone_lamp");
            blockRegistry.Register(new Block("minecraft:double_wooden_slab", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 125, "minecraft:double_wooden_slab");
            blockRegistry.Register(new Block("minecraft:wooden_slab", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 126, "minecraft:wooden_slab");
            blockRegistry.Register(new Block("minecraft:cocoa", true, true, 0.2, HarvestTier.Hand, BlockMaterial.Plant, false, false), 127, "minecraft:cocoa");
            blockRegistry.Register(new Block("minecraft:sandstone_stairs", true, true, 0.8, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 128, "minecraft:sandstone_stairs");
            blockRegistry.Register(new Block("minecraft:emerald_ore", true, true, 3, HarvestTier.Iron, BlockMaterial.Rock, false, false), 129, "minecraft:emerald_ore");
            blockRegistry.Register(new Block("minecraft:ender_chest", true, true, 22.5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 130, "minecraft:ender_chest");
            blockRegistry.Register(new Block("minecraft:tripwire_hook", false, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 131, "minecraft:tripwire_hook");
            blockRegistry.Register(new Block("minecraft:tripwire", false, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 132, "minecraft:tripwire");
            blockRegistry.Register(new Block("minecraft:emerald_block", true, true, 5, HarvestTier.Iron, BlockMaterial.Rock, false, false), 133, "minecraft:emerald_block");
            blockRegistry.Register(new Block("minecraft:spruce_stairs", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 134, "minecraft:spruce_stairs");
            blockRegistry.Register(new Block("minecraft:birch_stairs", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 135, "minecraft:birch_stairs");
            blockRegistry.Register(new Block("minecraft:jungle_stairs", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 136, "minecraft:jungle_stairs");
            blockRegistry.Register(new Block("minecraft:command_block", true, false, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 137, "minecraft:command_block");
            blockRegistry.Register(new Block("minecraft:beacon", true, true, 3, HarvestTier.Hand, BlockMaterial.Normal, false, false), 138, "minecraft:beacon");
            blockRegistry.Register(new Block("minecraft:cobblestone_wall", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 139, "minecraft:cobblestone_wall");
            blockRegistry.Register(new Block("minecraft:flower_pot", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 140, "minecraft:flower_pot");
            blockRegistry.Register(new Block("minecraft:carrots", false, true, 0, HarvestTier.Hand, BlockMaterial.Plant, false, false), 141, "minecraft:carrots");
            blockRegistry.Register(new Block("minecraft:potatoes", false, true, 0, HarvestTier.Hand, BlockMaterial.Plant, false, false), 142, "minecraft:potatoes");
            blockRegistry.Register(new Block("minecraft:wooden_button", false, true, 0.5, HarvestTier.Hand, BlockMaterial.Wood, false, false), 143, "minecraft:wooden_button");
            blockRegistry.Register(new Block("minecraft:skull", true, true, 1, HarvestTier.Hand, BlockMaterial.Normal, false, false), 144, "minecraft:skull");
            blockRegistry.Register(new Block("minecraft:anvil", true, true, 5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 145, "minecraft:anvil");
            blockRegistry.Register(new Block("minecraft:trapped_chest", true, true, 2.5, HarvestTier.Hand, BlockMaterial.Wood, false, false), 146, "minecraft:trapped_chest");
            blockRegistry.Register(new Block("minecraft:light_weighted_pressure_plate", false, true, 0.5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 147, "minecraft:light_weighted_pressure_plate");
            blockRegistry.Register(new Block("minecraft:heavy_weighted_pressure_plate", false, true, 0.5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 148, "minecraft:heavy_weighted_pressure_plate");
            blockRegistry.Register(new Block("minecraft:unpowered_comparator", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 149, "minecraft:unpowered_comparator");
            blockRegistry.Register(new Block("minecraft:powered_comparator", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 150, "minecraft:powered_comparator");
            blockRegistry.Register(new Block("minecraft:daylight_detector", true, true, 0.2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 151, "minecraft:daylight_detector");
            blockRegistry.Register(new Block("minecraft:redstone_block", true, true, 5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 152, "minecraft:redstone_block");
            blockRegistry.Register(new Block("minecraft:quartz_ore", true, true, 3, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 153, "minecraft:quartz_ore");
            blockRegistry.Register(new Block("minecraft:hopper", true, true, 3, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 154, "minecraft:hopper");
            blockRegistry.Register(new Block("minecraft:quartz_block", true, true, 0.8, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 155, "minecraft:quartz_block");
            blockRegistry.Register(new Block("minecraft:quartz_stairs", true, true, 0.8, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 156, "minecraft:quartz_stairs");
            blockRegistry.Register(new Block("minecraft:activator_rail", false, true, 0.7, HarvestTier.Hand, BlockMaterial.Rock, false, false), 157, "minecraft:activator_rail");
            blockRegistry.Register(new Block("minecraft:dropper", true, true, 3.5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 158, "minecraft:dropper");
            blockRegistry.Register(new Block("minecraft:stained_hardened_clay", true, true, 1.25, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 159, "minecraft:stained_hardened_clay");
            blockRegistry.Register(new Block("minecraft:stained_glass_pane", true, true, 0.3, HarvestTier.Hand, BlockMaterial.Normal, false, false), 160, "minecraft:stained_glass_pane");
            blockRegistry.Register(new Block("minecraft:leaves2", true, true, 0.2, HarvestTier.Hand, BlockMaterial.Plant, false, false), 161, "minecraft:leaves2");
            blockRegistry.Register(new Block("minecraft:log2", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 162, "minecraft:log2");
            blockRegistry.Register(new Block("minecraft:acacia_stairs", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 163, "minecraft:acacia_stairs");
            blockRegistry.Register(new Block("minecraft:dark_oak_stairs", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 164, "minecraft:dark_oak_stairs");
            blockRegistry.Register(new Block("minecraft:slime", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 165, "minecraft:slime");
            blockRegistry.Register(new Block("minecraft:barrier", true, false, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 166, "minecraft:barrier");
            blockRegistry.Register(new Block("minecraft:iron_trapdoor", true, true, 5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 167, "minecraft:iron_trapdoor");
            blockRegistry.Register(new Block("minecraft:prismarine", true, true, 1.5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 168, "minecraft:prismarine");
            blockRegistry.Register(new Block("minecraft:sea_lantern", true, true, 0.3, HarvestTier.Hand, BlockMaterial.Normal, false, false), 169, "minecraft:sea_lantern");
            blockRegistry.Register(new Block("minecraft:hay_block", true, true, 0.5, HarvestTier.Hand, BlockMaterial.Normal, false, false), 170, "minecraft:hay_block");
            blockRegistry.Register(new Block("minecraft:carpet", true, true, 0.1, HarvestTier.Hand, BlockMaterial.Normal, false, false), 171, "minecraft:carpet");
            blockRegistry.Register(new Block("minecraft:hardened_clay", true, true, 1.25, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 172, "minecraft:hardened_clay");
            blockRegistry.Register(new Block("minecraft:coal_block", true, true, 5, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 173, "minecraft:coal_block");
            blockRegistry.Register(new Block("minecraft:packed_ice", true, true, 0.5, HarvestTier.Hand, BlockMaterial.Rock, false, false), 174, "minecraft:packed_ice");
            blockRegistry.Register(new Block("minecraft:double_plant", false, true, 0, HarvestTier.Hand, BlockMaterial.Plant, false, false), 175, "minecraft:double_plant");
            blockRegistry.Register(new Block("minecraft:standing_banner", false, true, 1, HarvestTier.Hand, BlockMaterial.Wood, false, false), 176, "minecraft:standing_banner");
            blockRegistry.Register(new Block("minecraft:wall_banner", false, true, 1, HarvestTier.Hand, BlockMaterial.Wood, false, false), 177, "minecraft:wall_banner");
            blockRegistry.Register(new Block("minecraft:daylight_detector_inverted", true, true, 0.2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 178, "minecraft:daylight_detector_inverted");
            blockRegistry.Register(new Block("minecraft:red_sandstone", true, true, 0.8, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 179, "minecraft:red_sandstone");
            blockRegistry.Register(new Block("minecraft:red_sandstone_stairs", true, true, 0.8, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 180, "minecraft:red_sandstone_stairs");
            blockRegistry.Register(new Block("minecraft:double_stone_slab2", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 181, "minecraft:double_stone_slab2");
            blockRegistry.Register(new Block("minecraft:stone_slab2", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 182, "minecraft:stone_slab2");
            blockRegistry.Register(new Block("minecraft:spruce_fence_gate", true, true, 0, HarvestTier.Hand, BlockMaterial.Wood, false, false), 183, "minecraft:spruce_fence_gate");
            blockRegistry.Register(new Block("minecraft:birch_fence_gate", true, true, 0, HarvestTier.Hand, BlockMaterial.Wood, false, false), 184, "minecraft:birch_fence_gate");
            blockRegistry.Register(new Block("minecraft:jungle_fence_gate", true, true, 0, HarvestTier.Hand, BlockMaterial.Wood, false, false), 185, "minecraft:jungle_fence_gate");
            blockRegistry.Register(new Block("minecraft:dark_oak_fence_gate", true, true, 0, HarvestTier.Hand, BlockMaterial.Wood, false, false), 186, "minecraft:dark_oak_fence_gate");
            blockRegistry.Register(new Block("minecraft:acacia_fence_gate", true, true, 0, HarvestTier.Hand, BlockMaterial.Wood, false, false), 187, "minecraft:acacia_fence_gate");
            blockRegistry.Register(new Block("minecraft:spruce_fence", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 188, "minecraft:spruce_fence");
            blockRegistry.Register(new Block("minecraft:birch_fence", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 189, "minecraft:birch_fence");
            blockRegistry.Register(new Block("minecraft:jungle_fence", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 190, "minecraft:jungle_fence");
            blockRegistry.Register(new Block("minecraft:dark_oak_fence", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 191, "minecraft:dark_oak_fence");
            blockRegistry.Register(new Block("minecraft:acacia_fence", true, true, 2, HarvestTier.Hand, BlockMaterial.Wood, false, false), 192, "minecraft:acacia_fence");
            blockRegistry.Register(new Block("minecraft:spruce_door", true, true, 3, HarvestTier.Hand, BlockMaterial.Wood, false, false), 193, "minecraft:spruce_door");
            blockRegistry.Register(new Block("minecraft:birch_door", true, true, 3, HarvestTier.Hand, BlockMaterial.Wood, false, false), 194, "minecraft:birch_door");
            blockRegistry.Register(new Block("minecraft:jungle_door", true, true, 3, HarvestTier.Hand, BlockMaterial.Wood, false, false), 195, "minecraft:jungle_door");
            blockRegistry.Register(new Block("minecraft:acacia_door", true, true, 3, HarvestTier.Hand, BlockMaterial.Wood, false, false), 196, "minecraft:acacia_door");
            blockRegistry.Register(new Block("minecraft:dark_oak_door", true, true, 3, HarvestTier.Hand, BlockMaterial.Wood, false, false), 197, "minecraft:dark_oak_door");
            blockRegistry.Register(new Block("minecraft:end_rod", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 198, "minecraft:end_rod");
            blockRegistry.Register(new Block("minecraft:chorus_plant", true, true, 0.4, HarvestTier.Hand, BlockMaterial.Normal, false, false), 199, "minecraft:chorus_plant");
            blockRegistry.Register(new Block("minecraft:chorus_flower", true, true, 0.4, HarvestTier.Hand, BlockMaterial.Normal, false, false), 200, "minecraft:chorus_flower");
            blockRegistry.Register(new Block("minecraft:purpur_block", true, true, 1.5, HarvestTier.Wooden, BlockMaterial.Normal, false, false), 201, "minecraft:purpur_block");
            blockRegistry.Register(new Block("minecraft:purpur_pillar", true, true, 1.5, HarvestTier.Wooden, BlockMaterial.Normal, false, false), 202, "minecraft:purpur_pillar");
            blockRegistry.Register(new Block("minecraft:purpur_stairs", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 203, "minecraft:purpur_stairs");
            blockRegistry.Register(new Block("minecraft:purpur_double_slab", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 204, "minecraft:purpur_double_slab");
            blockRegistry.Register(new Block("minecraft:purpur_slab", true, true, 2, HarvestTier.Wooden, BlockMaterial.Rock, false, false), 205, "minecraft:purpur_slab");
            blockRegistry.Register(new Block("minecraft:end_bricks", true, true, 0.8, HarvestTier.Wooden, BlockMaterial.Normal, false, false), 206, "minecraft:end_bricks");
            blockRegistry.Register(new Block("minecraft:beetroots", false, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 207, "minecraft:beetroots");
            blockRegistry.Register(new Block("minecraft:grass_path", true, true, 0.6, HarvestTier.Hand, BlockMaterial.Plant, false, false), 208, "minecraft:grass_path");
            blockRegistry.Register(new Block("minecraft:end_gateway", false, false, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 209, "minecraft:end_gateway");
            blockRegistry.Register(new Block("minecraft:repeating_command_block", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 210, "minecraft:repeating_command_block");
            blockRegistry.Register(new Block("minecraft:chain_command_block", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 211, "minecraft:chain_command_block");
            blockRegistry.Register(new Block("minecraft:frosted_ice", true, true, 0.5, HarvestTier.Hand, BlockMaterial.Normal, false, false), 212, "minecraft:frosted_ice");
            blockRegistry.Register(new Block("minecraft:magma", true, true, 0.5, HarvestTier.Wooden, BlockMaterial.Normal, true, false), 213, "minecraft:magma");
            blockRegistry.Register(new Block("minecraft:nether_wart_block", true, true, 1, HarvestTier.Hand, BlockMaterial.Normal, false, false), 214, "minecraft:nether_wart_block");
            blockRegistry.Register(new Block("minecraft:red_nether_brick", true, true, 2, HarvestTier.Wooden, BlockMaterial.Normal, false, false), 215, "minecraft:red_nether_brick");
            blockRegistry.Register(new Block("minecraft:bone_block", true, true, 2, HarvestTier.Wooden, BlockMaterial.Normal, false, false), 216, "minecraft:bone_block");
            blockRegistry.Register(new Block("minecraft:structure_void", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 217, "minecraft:structure_void");
            blockRegistry.Register(new Block("minecraft:observer", true, true, 3.5, HarvestTier.Wooden, BlockMaterial.Normal, false, false), 218, "minecraft:observer");
            blockRegistry.Register(new Block("minecraft:white_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 219, "minecraft:white_shulker_box");
            blockRegistry.Register(new Block("minecraft:orange_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 220, "minecraft:orange_shulker_box");
            blockRegistry.Register(new Block("minecraft:magenta_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 221, "minecraft:magenta_shulker_box");
            blockRegistry.Register(new Block("minecraft:light_blue_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 222, "minecraft:light_blue_shulker_box");
            blockRegistry.Register(new Block("minecraft:yellow_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 223, "minecraft:yellow_shulker_box");
            blockRegistry.Register(new Block("minecraft:lime_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 224, "minecraft:lime_shulker_box");
            blockRegistry.Register(new Block("minecraft:pink_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 225, "minecraft:pink_shulker_box");
            blockRegistry.Register(new Block("minecraft:gray_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 226, "minecraft:gray_shulker_box");
            blockRegistry.Register(new Block("minecraft:light_gray_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 227, "minecraft:light_gray_shulker_box");
            blockRegistry.Register(new Block("minecraft:cyan_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 228, "minecraft:cyan_shulker_box");
            blockRegistry.Register(new Block("minecraft:purple_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 229, "minecraft:purple_shulker_box");
            blockRegistry.Register(new Block("minecraft:blue_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 230, "minecraft:blue_shulker_box");
            blockRegistry.Register(new Block("minecraft:brown_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 231, "minecraft:brown_shulker_box");
            blockRegistry.Register(new Block("minecraft:green_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 232, "minecraft:green_shulker_box");
            blockRegistry.Register(new Block("minecraft:red_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 233, "minecraft:red_shulker_box");
            blockRegistry.Register(new Block("minecraft:black_shulker_box", true, true, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 234, "minecraft:black_shulker_box");
            blockRegistry.Register(new Block("minecraft:structure_block", true, false, 0, HarvestTier.Hand, BlockMaterial.Normal, false, false), 255, "minecraft:structure_block");
        }

        private static void RegisterVanillaItems(IGameObjectRegistry<IItemStack> itemRegistry)
        {
            itemRegistry.Register(new HarvestingTool("yksmc:hand", 1, HarvestTier.Hand, 1, BlockMaterial.Normal), "yksmc:hand");
            itemRegistry.Register(new ItemStack("yksmc:empty", 0, 0), -1, "yksmc:empty");

            // Blocks
            itemRegistry.Register(new ItemStack("minecraft:air", 64, 1), 0, "minecraft:air");
            itemRegistry.Register(new ItemStack("minecraft:stone", 64, 1), 1, "minecraft:stone");
            itemRegistry.Register(new ItemStack("minecraft:grass", 64, 1), 2, "minecraft:grass");
            itemRegistry.Register(new ItemStack("minecraft:dirt", 64, 1), 3, "minecraft:dirt");
            itemRegistry.Register(new ItemStack("minecraft:cobblestone", 64, 1), 4, "minecraft:cobblestone");
            itemRegistry.Register(new ItemStack("minecraft:planks", 64, 1), 5, "minecraft:planks");
            itemRegistry.Register(new ItemStack("minecraft:sapling", 64, 1), 6, "minecraft:sapling");
            itemRegistry.Register(new ItemStack("minecraft:bedrock", 64, 1), 7, "minecraft:bedrock");

            itemRegistry.Register(new ItemStack("minecraft:sand", 64, 1), 12, "minecraft:sand");
            itemRegistry.Register(new ItemStack("minecraft:gravel", 64, 1), 13, "minecraft:gravel");
            itemRegistry.Register(new ItemStack("minecraft:gold_ore", 64, 1), 14, "minecraft:gold_ore");
            itemRegistry.Register(new ItemStack("minecraft:iron_ore", 64, 1), 15, "minecraft:iron_ore");
            itemRegistry.Register(new ItemStack("minecraft:coal_ore", 64, 1), 16, "minecraft:coal_ore");
            itemRegistry.Register(new ItemStack("minecraft:log", 64, 1), 17, "minecraft:log");
            itemRegistry.Register(new ItemStack("minecraft:leaves", 64, 1), 18, "minecraft:leaves");
            itemRegistry.Register(new ItemStack("minecraft:sponge", 64, 1), 19, "minecraft:sponge");
            itemRegistry.Register(new ItemStack("minecraft:glass", 64, 1), 20, "minecraft:glass");
            itemRegistry.Register(new ItemStack("minecraft:lapis_ore", 64, 1), 21, "minecraft:lapis_ore");
            itemRegistry.Register(new ItemStack("minecraft:lapis_block", 64, 1), 22, "minecraft:lapis_block");
            itemRegistry.Register(new ItemStack("minecraft:dispenser", 64, 1), 23, "minecraft:dispenser");
            itemRegistry.Register(new ItemStack("minecraft:sandstone", 64, 1), 24, "minecraft:sandstone");
            itemRegistry.Register(new ItemStack("minecraft:noteblock", 64, 1), 25, "minecraft:noteblock");

            itemRegistry.Register(new ItemStack("minecraft:golden_rail", 64, 1), 27, "minecraft:golden_rail");
            itemRegistry.Register(new ItemStack("minecraft:detector_rail", 64, 1), 28, "minecraft:detector_rail");
            itemRegistry.Register(new ItemStack("minecraft:sticky_piston", 64, 1), 29, "minecraft:sticky_piston");
            itemRegistry.Register(new ItemStack("minecraft:web", 64, 1), 30, "minecraft:web");
            itemRegistry.Register(new ItemStack("minecraft:tallgrass", 64, 1), 31, "minecraft:tallgrass");
            itemRegistry.Register(new ItemStack("minecraft:deadbush", 64, 1), 32, "minecraft:deadbush");
            itemRegistry.Register(new ItemStack("minecraft:piston", 64, 1), 33, "minecraft:piston");

            itemRegistry.Register(new ItemStack("minecraft:wool", 64, 1), 35, "minecraft:wool");

            itemRegistry.Register(new ItemStack("minecraft:yellow_flower", 64, 1), 37, "minecraft:yellow_flower");
            itemRegistry.Register(new ItemStack("minecraft:red_flower", 64, 1), 38, "minecraft:red_flower");
            itemRegistry.Register(new ItemStack("minecraft:brown_mushroom", 64, 1), 39, "minecraft:brown_mushroom");
            itemRegistry.Register(new ItemStack("minecraft:red_mushroom", 64, 1), 40, "minecraft:red_mushroom");
            itemRegistry.Register(new ItemStack("minecraft:gold_block", 64, 1), 41, "minecraft:gold_block");
            itemRegistry.Register(new ItemStack("minecraft:iron_block", 64, 1), 42, "minecraft:iron_block");
            itemRegistry.Register(new ItemStack("minecraft:double_stone_slab", 64, 1), 43, "minecraft:double_stone_slab");
            itemRegistry.Register(new ItemStack("minecraft:stone_slab", 64, 1), 44, "minecraft:stone_slab");
            itemRegistry.Register(new ItemStack("minecraft:brick_block", 64, 1), 45, "minecraft:brick_block");
            itemRegistry.Register(new ItemStack("minecraft:tnt", 64, 1), 46, "minecraft:tnt");
            itemRegistry.Register(new ItemStack("minecraft:bookshelf", 64, 1), 47, "minecraft:bookshelf");
            itemRegistry.Register(new ItemStack("minecraft:mossy_cobblestone", 64, 1), 48, "minecraft:mossy_cobblestone");
            itemRegistry.Register(new ItemStack("minecraft:obsidian", 64, 1), 49, "minecraft:obsidian");
            itemRegistry.Register(new ItemStack("minecraft:torch", 64, 1), 50, "minecraft:torch");

            itemRegistry.Register(new ItemStack("minecraft:oak_stairs", 64, 1), 53, "minecraft:oak_stairs");
            itemRegistry.Register(new ItemStack("minecraft:chest", 64, 1), 54, "minecraft:chest");
            itemRegistry.Register(new ItemStack("minecraft:redstone_wire", 64, 1), 55, "minecraft:redstone_wire");
            itemRegistry.Register(new ItemStack("minecraft:diamond_ore", 64, 1), 56, "minecraft:diamond_ore");
            itemRegistry.Register(new ItemStack("minecraft:diamond_block", 64, 1), 57, "minecraft:diamond_block");
            itemRegistry.Register(new ItemStack("minecraft:crafting_table", 64, 1), 58, "minecraft:crafting_table");

            itemRegistry.Register(new ItemStack("minecraft:farmland", 64, 1), 60, "minecraft:farmland");
            itemRegistry.Register(new ItemStack("minecraft:furnace", 64, 1), 61, "minecraft:furnace");

            itemRegistry.Register(new ItemStack("minecraft:ladder", 64, 1), 65, "minecraft:ladder");
            itemRegistry.Register(new ItemStack("minecraft:rail", 64, 1), 66, "minecraft:rail");
            itemRegistry.Register(new ItemStack("minecraft:stone_stairs", 64, 1), 67, "minecraft:stone_stairs");

            itemRegistry.Register(new ItemStack("minecraft:lever", 64, 1), 69, "minecraft:lever");
            itemRegistry.Register(new ItemStack("minecraft:stone_pressure_plate", 64, 1), 70, "minecraft:stone_pressure_plate");

            itemRegistry.Register(new ItemStack("minecraft:wooden_pressure_plate", 64, 1), 72, "minecraft:wooden_pressure_plate");
            itemRegistry.Register(new ItemStack("minecraft:redstone_ore", 64, 1), 73, "minecraft:redstone_ore");

            itemRegistry.Register(new ItemStack("minecraft:redstone_torch", 64, 1), 76, "minecraft:redstone_torch");
            itemRegistry.Register(new ItemStack("minecraft:stone_button", 64, 1), 77, "minecraft:stone_button");
            itemRegistry.Register(new ItemStack("minecraft:snow_layer", 64, 1), 78, "minecraft:snow_layer");
            itemRegistry.Register(new ItemStack("minecraft:ice", 64, 1), 79, "minecraft:ice");
            itemRegistry.Register(new ItemStack("minecraft:snow", 64, 1), 80, "minecraft:snow");
            itemRegistry.Register(new ItemStack("minecraft:cactus", 64, 1), 81, "minecraft:cactus");
            itemRegistry.Register(new ItemStack("minecraft:clay", 64, 1), 82, "minecraft:clay");

            itemRegistry.Register(new ItemStack("minecraft:jukebox", 64, 1), 84, "minecraft:jukebox");
            itemRegistry.Register(new ItemStack("minecraft:fence", 64, 1), 85, "minecraft:fence");
            itemRegistry.Register(new ItemStack("minecraft:pumpkin", 64, 1), 86, "minecraft:pumpkin");
            itemRegistry.Register(new ItemStack("minecraft:netherrack", 64, 1), 87, "minecraft:netherrack");
            itemRegistry.Register(new ItemStack("minecraft:soul_sand", 64, 1), 88, "minecraft:soul_sand");
            itemRegistry.Register(new ItemStack("minecraft:glowstone", 64, 1), 89, "minecraft:glowstone");

            itemRegistry.Register(new ItemStack("minecraft:lit_pumpkin", 64, 1), 91, "minecraft:lit_pumpkin");

            itemRegistry.Register(new ItemStack("minecraft:stained_glass", 64, 1), 95, "minecraft:stained_glass");
            itemRegistry.Register(new ItemStack("minecraft:trapdoor", 64, 1), 96, "minecraft:trapdoor");
            itemRegistry.Register(new ItemStack("minecraft:monster_egg", 64, 1), 97, "minecraft:monster_egg");
            itemRegistry.Register(new ItemStack("minecraft:stonebrick", 64, 1), 98, "minecraft:stonebrick");
            itemRegistry.Register(new ItemStack("minecraft:brown_mushroom_block", 64, 1), 99, "minecraft:brown_mushroom_block");
            itemRegistry.Register(new ItemStack("minecraft:red_mushroom_block", 64, 1), 100, "minecraft:red_mushroom_block");
            itemRegistry.Register(new ItemStack("minecraft:iron_bars", 64, 1), 101, "minecraft:iron_bars");
            itemRegistry.Register(new ItemStack("minecraft:glass_pane", 64, 1), 102, "minecraft:glass_pane");
            itemRegistry.Register(new ItemStack("minecraft:melon_block", 64, 1), 103, "minecraft:melon_block");

            itemRegistry.Register(new ItemStack("minecraft:vine", 64, 1), 106, "minecraft:vine");
            itemRegistry.Register(new ItemStack("minecraft:fence_gate", 64, 1), 107, "minecraft:fence_gate");
            itemRegistry.Register(new ItemStack("minecraft:brick_stairs", 64, 1), 108, "minecraft:brick_stairs");
            itemRegistry.Register(new ItemStack("minecraft:stone_brick_stairs", 64, 1), 109, "minecraft:stone_brick_stairs");
            itemRegistry.Register(new ItemStack("minecraft:mycelium", 64, 1), 110, "minecraft:mycelium");
            itemRegistry.Register(new ItemStack("minecraft:waterlily", 64, 1), 111, "minecraft:waterlily");
            itemRegistry.Register(new ItemStack("minecraft:nether_brick", 64, 1), 112, "minecraft:nether_brick");
            itemRegistry.Register(new ItemStack("minecraft:nether_brick_fence", 64, 1), 113, "minecraft:nether_brick_fence");
            itemRegistry.Register(new ItemStack("minecraft:nether_brick_stairs", 64, 1), 114, "minecraft:nether_brick_stairs");

            itemRegistry.Register(new ItemStack("minecraft:enchanting_table", 64, 1), 116, "minecraft:enchanting_table");

            itemRegistry.Register(new ItemStack("minecraft:end_portal_frame", 64, 1), 120, "minecraft:end_portal_frame");
            itemRegistry.Register(new ItemStack("minecraft:end_stone", 64, 1), 121, "minecraft:end_stone");
            itemRegistry.Register(new ItemStack("minecraft:dragon_egg", 64, 1), 122, "minecraft:dragon_egg");
            itemRegistry.Register(new ItemStack("minecraft:redstone_lamp", 64, 1), 123, "minecraft:redstone_lamp");
            itemRegistry.Register(new ItemStack("minecraft:double_wooden_slab", 64, 1), 125, "minecraft:double_wooden_slab");
            itemRegistry.Register(new ItemStack("minecraft:wooden_slab", 64, 1), 126, "minecraft:wooden_slab");

            itemRegistry.Register(new ItemStack("minecraft:sandstone_stairs", 64, 1), 128, "minecraft:sandstone_stairs");
            itemRegistry.Register(new ItemStack("minecraft:emerald_ore", 64, 1), 129, "minecraft:emerald_ore");
            itemRegistry.Register(new ItemStack("minecraft:ender_chest", 64, 1), 130, "minecraft:ender_chest");
            itemRegistry.Register(new ItemStack("minecraft:tripwire_hook", 64, 1), 131, "minecraft:tripwire_hook");

            itemRegistry.Register(new ItemStack("minecraft:emerald_block", 64, 1), 133, "minecraft:emerald_block");
            itemRegistry.Register(new ItemStack("minecraft:spruce_stairs", 64, 1), 134, "minecraft:spruce_stairs");
            itemRegistry.Register(new ItemStack("minecraft:birch_stairs", 64, 1), 135, "minecraft:birch_stairs");
            itemRegistry.Register(new ItemStack("minecraft:jungle_stairs", 64, 1), 136, "minecraft:jungle_stairs");
            itemRegistry.Register(new ItemStack("minecraft:command_block", 64, 1), 137, "minecraft:command_block");
            itemRegistry.Register(new ItemStack("minecraft:beacon", 64, 1), 138, "minecraft:beacon");
            itemRegistry.Register(new ItemStack("minecraft:cobblestone_wall", 64, 1), 139, "minecraft:cobblestone_wall");

            itemRegistry.Register(new ItemStack("minecraft:carrots", 64, 1), 141, "minecraft:carrots");
            itemRegistry.Register(new ItemStack("minecraft:potatoes", 64, 1), 142, "minecraft:potatoes");
            itemRegistry.Register(new ItemStack("minecraft:wooden_button", 64, 1), 143, "minecraft:wooden_button");

            itemRegistry.Register(new ItemStack("minecraft:anvil", 64, 1), 145, "minecraft:anvil");
            itemRegistry.Register(new ItemStack("minecraft:trapped_chest", 64, 1), 146, "minecraft:trapped_chest");
            itemRegistry.Register(new ItemStack("minecraft:light_weighted_pressure_plate", 64, 1), 147, "minecraft:light_weighted_pressure_plate");
            itemRegistry.Register(new ItemStack("minecraft:heavy_weighted_pressure_plate", 64, 1), 148, "minecraft:heavy_weighted_pressure_plate");

            itemRegistry.Register(new ItemStack("minecraft:daylight_detector", 64, 1), 151, "minecraft:daylight_detector");
            itemRegistry.Register(new ItemStack("minecraft:redstone_block", 64, 1), 152, "minecraft:redstone_block");
            itemRegistry.Register(new ItemStack("minecraft:quartz_ore", 64, 1), 153, "minecraft:quartz_ore");
            itemRegistry.Register(new ItemStack("minecraft:hopper", 64, 1), 154, "minecraft:hopper");
            itemRegistry.Register(new ItemStack("minecraft:quartz_block", 64, 1), 155, "minecraft:quartz_block");
            itemRegistry.Register(new ItemStack("minecraft:quartz_stairs", 64, 1), 156, "minecraft:quartz_stairs");
            itemRegistry.Register(new ItemStack("minecraft:activator_rail", 64, 1), 157, "minecraft:activator_rail");
            itemRegistry.Register(new ItemStack("minecraft:dropper", 64, 1), 158, "minecraft:dropper");
            itemRegistry.Register(new ItemStack("minecraft:stained_hardened_clay", 64, 1), 159, "minecraft:stained_hardened_clay");
            itemRegistry.Register(new ItemStack("minecraft:stained_glass_pane", 64, 1), 160, "minecraft:stained_glass_pane");
            itemRegistry.Register(new ItemStack("minecraft:leaves2", 64, 1), 161, "minecraft:leaves2");
            itemRegistry.Register(new ItemStack("minecraft:log2", 64, 1), 162, "minecraft:log2");
            itemRegistry.Register(new ItemStack("minecraft:acacia_stairs", 64, 1), 163, "minecraft:acacia_stairs");
            itemRegistry.Register(new ItemStack("minecraft:dark_oak_stairs", 64, 1), 164, "minecraft:dark_oak_stairs");
            itemRegistry.Register(new ItemStack("minecraft:slime", 64, 1), 165, "minecraft:slime");
            itemRegistry.Register(new ItemStack("minecraft:barrier", 64, 1), 166, "minecraft:barrier");
            itemRegistry.Register(new ItemStack("minecraft:iron_trapdoor", 64, 1), 167, "minecraft:iron_trapdoor");
            itemRegistry.Register(new ItemStack("minecraft:prismarine", 64, 1), 168, "minecraft:prismarine");
            itemRegistry.Register(new ItemStack("minecraft:sea_lantern", 64, 1), 169, "minecraft:sea_lantern");
            itemRegistry.Register(new ItemStack("minecraft:hay_block", 64, 1), 170, "minecraft:hay_block");
            itemRegistry.Register(new ItemStack("minecraft:carpet", 64, 1), 171, "minecraft:carpet");
            itemRegistry.Register(new ItemStack("minecraft:hardened_clay", 64, 1), 172, "minecraft:hardened_clay");
            itemRegistry.Register(new ItemStack("minecraft:coal_block", 64, 1), 173, "minecraft:coal_block");
            itemRegistry.Register(new ItemStack("minecraft:packed_ice", 64, 1), 174, "minecraft:packed_ice");
            itemRegistry.Register(new ItemStack("minecraft:double_plant", 64, 1), 175, "minecraft:double_plant");
            itemRegistry.Register(new ItemStack("minecraft:standing_banner", 64, 1), 176, "minecraft:standing_banner");
            itemRegistry.Register(new ItemStack("minecraft:wall_banner", 64, 1), 177, "minecraft:wall_banner");

            itemRegistry.Register(new ItemStack("minecraft:red_sandstone", 64, 1), 179, "minecraft:red_sandstone");
            itemRegistry.Register(new ItemStack("minecraft:red_sandstone_stairs", 64, 1), 180, "minecraft:red_sandstone_stairs");
            itemRegistry.Register(new ItemStack("minecraft:double_stone_slab2", 64, 1), 181, "minecraft:double_stone_slab2");
            itemRegistry.Register(new ItemStack("minecraft:stone_slab2", 64, 1), 182, "minecraft:stone_slab2");
            itemRegistry.Register(new ItemStack("minecraft:spruce_fence_gate", 64, 1), 183, "minecraft:spruce_fence_gate");
            itemRegistry.Register(new ItemStack("minecraft:birch_fence_gate", 64, 1), 184, "minecraft:birch_fence_gate");
            itemRegistry.Register(new ItemStack("minecraft:jungle_fence_gate", 64, 1), 185, "minecraft:jungle_fence_gate");
            itemRegistry.Register(new ItemStack("minecraft:dark_oak_fence_gate", 64, 1), 186, "minecraft:dark_oak_fence_gate");
            itemRegistry.Register(new ItemStack("minecraft:acacia_fence_gate", 64, 1), 187, "minecraft:acacia_fence_gate");
            itemRegistry.Register(new ItemStack("minecraft:spruce_fence", 64, 1), 188, "minecraft:spruce_fence");
            itemRegistry.Register(new ItemStack("minecraft:birch_fence", 64, 1), 189, "minecraft:birch_fence");
            itemRegistry.Register(new ItemStack("minecraft:jungle_fence", 64, 1), 190, "minecraft:jungle_fence");
            itemRegistry.Register(new ItemStack("minecraft:dark_oak_fence", 64, 1), 191, "minecraft:dark_oak_fence");
            itemRegistry.Register(new ItemStack("minecraft:acacia_fence", 64, 1), 192, "minecraft:acacia_fence");

            itemRegistry.Register(new ItemStack("minecraft:end_rod", 64, 1), 198, "minecraft:end_rod");
            itemRegistry.Register(new ItemStack("minecraft:chorus_plant", 64, 1), 199, "minecraft:chorus_plant");
            itemRegistry.Register(new ItemStack("minecraft:chorus_flower", 64, 1), 200, "minecraft:chorus_flower");
            itemRegistry.Register(new ItemStack("minecraft:purpur_block", 64, 1), 201, "minecraft:purpur_block");
            itemRegistry.Register(new ItemStack("minecraft:purpur_pillar", 64, 1), 202, "minecraft:purpur_pillar");
            itemRegistry.Register(new ItemStack("minecraft:purpur_stairs", 64, 1), 203, "minecraft:purpur_stairs");
            itemRegistry.Register(new ItemStack("minecraft:purpur_double_slab", 64, 1), 204, "minecraft:purpur_double_slab");
            itemRegistry.Register(new ItemStack("minecraft:purpur_slab", 64, 1), 205, "minecraft:purpur_slab");
            itemRegistry.Register(new ItemStack("minecraft:end_bricks", 64, 1), 206, "minecraft:end_bricks");
            itemRegistry.Register(new ItemStack("minecraft:beetroots", 64, 1), 207, "minecraft:beetroots");
            itemRegistry.Register(new ItemStack("minecraft:grass_path", 64, 1), 208, "minecraft:grass_path");
            itemRegistry.Register(new ItemStack("minecraft:end_gateway", 64, 1), 209, "minecraft:end_gateway");
            itemRegistry.Register(new ItemStack("minecraft:repeating_command_block", 64, 1), 210, "minecraft:repeating_command_block");
            itemRegistry.Register(new ItemStack("minecraft:chain_command_block", 64, 1), 211, "minecraft:chain_command_block");
            itemRegistry.Register(new ItemStack("minecraft:frosted_ice", 64, 1), 212, "minecraft:frosted_ice");
            itemRegistry.Register(new ItemStack("minecraft:magma", 64, 1), 213, "minecraft:magma");
            itemRegistry.Register(new ItemStack("minecraft:nether_wart_block", 64, 1), 214, "minecraft:nether_wart_block");
            itemRegistry.Register(new ItemStack("minecraft:red_nether_brick", 64, 1), 215, "minecraft:red_nether_brick");
            itemRegistry.Register(new ItemStack("minecraft:bone_block", 64, 1), 216, "minecraft:bone_block");
            itemRegistry.Register(new ItemStack("minecraft:structure_void", 64, 1), 217, "minecraft:structure_void");
            itemRegistry.Register(new ItemStack("minecraft:observer", 64, 1), 218, "minecraft:observer");
            itemRegistry.Register(new ItemStack("minecraft:white_shulker_box", 64, 1), 219, "minecraft:white_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:orange_shulker_box", 64, 1), 220, "minecraft:orange_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:magenta_shulker_box", 64, 1), 221, "minecraft:magenta_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:light_blue_shulker_box", 64, 1), 222, "minecraft:light_blue_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:yellow_shulker_box", 64, 1), 223, "minecraft:yellow_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:lime_shulker_box", 64, 1), 224, "minecraft:lime_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:pink_shulker_box", 64, 1), 225, "minecraft:pink_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:gray_shulker_box", 64, 1), 226, "minecraft:gray_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:light_gray_shulker_box", 64, 1), 227, "minecraft:light_gray_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:cyan_shulker_box", 64, 1), 228, "minecraft:cyan_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:purple_shulker_box", 64, 1), 229, "minecraft:purple_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:blue_shulker_box", 64, 1), 230, "minecraft:blue_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:brown_shulker_box", 64, 1), 231, "minecraft:brown_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:green_shulker_box", 64, 1), 232, "minecraft:green_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:red_shulker_box", 64, 1), 233, "minecraft:red_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:black_shulker_box", 64, 1), 234, "minecraft:black_shulker_box");
            itemRegistry.Register(new ItemStack("minecraft:structure_block", 64, 1), 255, "minecraft:structure_block");
            //Items
            itemRegistry.Register(new ItemStack("minecraft:flint_and_steel", 1, 1), 259, "minecraft:flint_and_steel");
            itemRegistry.Register(new ItemStack("minecraft:apple", 64, 1), 260, "minecraft:apple");
            itemRegistry.Register(new ItemStack("minecraft:bow", 1, 1), 261, "minecraft:bow");
            itemRegistry.Register(new ItemStack("minecraft:arrow", 64, 1), 262, "minecraft:arrow");
            itemRegistry.Register(new ItemStack("minecraft:coal", 64, 1), 263, "minecraft:coal");
            itemRegistry.Register(new ItemStack("minecraft:diamond", 64, 1), 264, "minecraft:diamond");
            itemRegistry.Register(new ItemStack("minecraft:iron_ingot", 64, 1), 265, "minecraft:iron_ingot");
            itemRegistry.Register(new ItemStack("minecraft:gold_ingot", 64, 1), 266, "minecraft:gold_ingot");
            itemRegistry.Register(new ItemStack("minecraft:iron_sword", 1, 1), 267, "minecraft:iron_sword");
            itemRegistry.Register(new ItemStack("minecraft:wooden_sword", 1, 1), 268, "minecraft:wooden_sword");
            itemRegistry.Register(new ItemStack("minecraft:stone_sword", 1, 1), 272, "minecraft:stone_sword");
            itemRegistry.Register(new ItemStack("minecraft:diamond_sword", 1, 1), 276, "minecraft:diamond_sword");
            itemRegistry.Register(new ItemStack("minecraft:stick", 64, 1), 280, "minecraft:stick");
            itemRegistry.Register(new ItemStack("minecraft:bowl", 64, 1), 281, "minecraft:bowl");
            itemRegistry.Register(new ItemStack("minecraft:mushroom_stew", 1, 1), 282, "minecraft:mushroom_stew");
            itemRegistry.Register(new ItemStack("minecraft:golden_sword", 1, 1), 283, "minecraft:golden_sword");
            itemRegistry.Register(new ItemStack("minecraft:string", 64, 1), 287, "minecraft:string");
            itemRegistry.Register(new ItemStack("minecraft:feather", 64, 1), 288, "minecraft:feather");
            itemRegistry.Register(new ItemStack("minecraft:gunpowder", 64, 1), 289, "minecraft:gunpowder");
            itemRegistry.Register(new ItemStack("minecraft:wooden_hoe", 1, 1), 290, "minecraft:wooden_hoe");
            itemRegistry.Register(new ItemStack("minecraft:stone_hoe", 1, 1), 291, "minecraft:stone_hoe");
            itemRegistry.Register(new ItemStack("minecraft:iron_hoe", 1, 1), 292, "minecraft:iron_hoe");
            itemRegistry.Register(new ItemStack("minecraft:diamond_hoe", 1, 1), 293, "minecraft:diamond_hoe");
            itemRegistry.Register(new ItemStack("minecraft:golden_hoe", 1, 1), 294, "minecraft:golden_hoe");
            itemRegistry.Register(new ItemStack("minecraft:wheat_seeds", 64, 1), 295, "minecraft:wheat_seeds");
            itemRegistry.Register(new ItemStack("minecraft:wheat", 64, 1), 296, "minecraft:wheat");
            itemRegistry.Register(new ItemStack("minecraft:bread", 64, 1), 297, "minecraft:bread");
            itemRegistry.Register(new ItemStack("minecraft:leather_helmet", 1, 1), 298, "minecraft:leather_helmet");
            itemRegistry.Register(new ItemStack("minecraft:leather_chestplate", 1, 1), 299, "minecraft:leather_chestplate");
            itemRegistry.Register(new ItemStack("minecraft:leather_leggings", 1, 1), 300, "minecraft:leather_leggings");
            itemRegistry.Register(new ItemStack("minecraft:leather_boots", 1, 1), 301, "minecraft:leather_boots");
            itemRegistry.Register(new ItemStack("minecraft:chainmail_helmet", 1, 1), 302, "minecraft:chainmail_helmet");
            itemRegistry.Register(new ItemStack("minecraft:chainmail_chestplate", 1, 1), 303, "minecraft:chainmail_chestplate");
            itemRegistry.Register(new ItemStack("minecraft:chainmail_leggings", 1, 1), 304, "minecraft:chainmail_leggings");
            itemRegistry.Register(new ItemStack("minecraft:chainmail_boots", 1, 1), 305, "minecraft:chainmail_boots");
            itemRegistry.Register(new ItemStack("minecraft:iron_helmet", 1, 1), 306, "minecraft:iron_helmet");
            itemRegistry.Register(new ItemStack("minecraft:iron_chestplate", 1, 1), 307, "minecraft:iron_chestplate");
            itemRegistry.Register(new ItemStack("minecraft:iron_leggings", 1, 1), 308, "minecraft:iron_leggings");
            itemRegistry.Register(new ItemStack("minecraft:iron_boots", 1, 1), 309, "minecraft:iron_boots");
            itemRegistry.Register(new ItemStack("minecraft:diamond_helmet", 1, 1), 310, "minecraft:diamond_helmet");
            itemRegistry.Register(new ItemStack("minecraft:diamond_chestplate", 1, 1), 311, "minecraft:diamond_chestplate");
            itemRegistry.Register(new ItemStack("minecraft:diamond_leggings", 1, 1), 312, "minecraft:diamond_leggings");
            itemRegistry.Register(new ItemStack("minecraft:diamond_boots", 1, 1), 313, "minecraft:diamond_boots");
            itemRegistry.Register(new ItemStack("minecraft:golden_helmet", 1, 1), 314, "minecraft:golden_helmet");
            itemRegistry.Register(new ItemStack("minecraft:golden_chestplate", 1, 1), 315, "minecraft:golden_chestplate");
            itemRegistry.Register(new ItemStack("minecraft:golden_leggings", 1, 1), 316, "minecraft:golden_leggings");
            itemRegistry.Register(new ItemStack("minecraft:golden_boots", 1, 1), 317, "minecraft:golden_boots");
            itemRegistry.Register(new ItemStack("minecraft:flint", 64, 1), 318, "minecraft:flint");
            itemRegistry.Register(new ItemStack("minecraft:porkchop", 64, 1), 319, "minecraft:porkchop");
            itemRegistry.Register(new ItemStack("minecraft:cooked_porkchop", 64, 1), 320, "minecraft:cooked_porkchop");
            itemRegistry.Register(new ItemStack("minecraft:painting", 64, 1), 321, "minecraft:painting");
            itemRegistry.Register(new ItemStack("minecraft:golden_apple", 64, 1), 322, "minecraft:golden_apple");
            itemRegistry.Register(new ItemStack("minecraft:sign", 16, 1), 323, "minecraft:sign");
            itemRegistry.Register(new ItemStack("minecraft:wooden_door", 64, 1), 324, "minecraft:wooden_door");
            itemRegistry.Register(new ItemStack("minecraft:bucket", 16, 1), 325, "minecraft:bucket");
            itemRegistry.Register(new ItemStack("minecraft:water_bucket", 1, 1), 326, "minecraft:water_bucket");
            itemRegistry.Register(new ItemStack("minecraft:lava_bucket", 1, 1), 327, "minecraft:lava_bucket");
            itemRegistry.Register(new ItemStack("minecraft:minecart", 1, 1), 328, "minecraft:minecart");
            itemRegistry.Register(new ItemStack("minecraft:saddle", 1, 1), 329, "minecraft:saddle");
            itemRegistry.Register(new ItemStack("minecraft:iron_door", 64, 1), 330, "minecraft:iron_door");
            itemRegistry.Register(new ItemStack("minecraft:redstone", 64, 1), 331, "minecraft:redstone");
            itemRegistry.Register(new ItemStack("minecraft:snowball", 16, 1), 332, "minecraft:snowball");
            itemRegistry.Register(new ItemStack("minecraft:boat", 1, 1), 333, "minecraft:boat");
            itemRegistry.Register(new ItemStack("minecraft:leather", 64, 1), 334, "minecraft:leather");
            itemRegistry.Register(new ItemStack("minecraft:milk_bucket", 1, 1), 335, "minecraft:milk_bucket");
            itemRegistry.Register(new ItemStack("minecraft:brick", 64, 1), 336, "minecraft:brick");
            itemRegistry.Register(new ItemStack("minecraft:clay_ball", 64, 1), 337, "minecraft:clay_ball");
            itemRegistry.Register(new ItemStack("minecraft:reeds", 64, 1), 338, "minecraft:reeds");
            itemRegistry.Register(new ItemStack("minecraft:paper", 64, 1), 339, "minecraft:paper");
            itemRegistry.Register(new ItemStack("minecraft:book", 64, 1), 340, "minecraft:book");
            itemRegistry.Register(new ItemStack("minecraft:slime_ball", 64, 1), 341, "minecraft:slime_ball");
            itemRegistry.Register(new ItemStack("minecraft:chest_minecart", 1, 1), 342, "minecraft:chest_minecart");
            itemRegistry.Register(new ItemStack("minecraft:furnace_minecart", 1, 1), 343, "minecraft:furnace_minecart");
            itemRegistry.Register(new ItemStack("minecraft:egg", 16, 1), 344, "minecraft:egg");
            itemRegistry.Register(new ItemStack("minecraft:compass", 64, 1), 345, "minecraft:compass");
            itemRegistry.Register(new ItemStack("minecraft:fishing_rod", 1, 1), 346, "minecraft:fishing_rod");
            itemRegistry.Register(new ItemStack("minecraft:clock", 64, 1), 347, "minecraft:clock");
            itemRegistry.Register(new ItemStack("minecraft:glowstone_dust", 64, 1), 348, "minecraft:glowstone_dust");
            itemRegistry.Register(new ItemStack("minecraft:fish", 64, 1), 349, "minecraft:fish");
            itemRegistry.Register(new ItemStack("minecraft:cooked_fish", 64, 1), 350, "minecraft:cooked_fish");
            itemRegistry.Register(new ItemStack("minecraft:dye", 64, 1), 351, "minecraft:dye");
            itemRegistry.Register(new ItemStack("minecraft:bone", 64, 1), 352, "minecraft:bone");
            itemRegistry.Register(new ItemStack("minecraft:sugar", 64, 1), 353, "minecraft:sugar");
            itemRegistry.Register(new ItemStack("minecraft:cake", 1, 1), 354, "minecraft:cake");
            itemRegistry.Register(new ItemStack("minecraft:bed", 1, 1), 355, "minecraft:bed");
            itemRegistry.Register(new ItemStack("minecraft:repeater", 64, 1), 356, "minecraft:repeater");
            itemRegistry.Register(new ItemStack("minecraft:cookie", 64, 1), 357, "minecraft:cookie");
            itemRegistry.Register(new ItemStack("minecraft:filled_map", 64, 1), 358, "minecraft:filled_map");
            itemRegistry.Register(new ItemStack("minecraft:shears", 1, 1), 359, "minecraft:shears");
            itemRegistry.Register(new ItemStack("minecraft:melon", 64, 1), 360, "minecraft:melon");
            itemRegistry.Register(new ItemStack("minecraft:pumpkin_seeds", 64, 1), 361, "minecraft:pumpkin_seeds");
            itemRegistry.Register(new ItemStack("minecraft:melon_seeds", 64, 1), 362, "minecraft:melon_seeds");
            itemRegistry.Register(new ItemStack("minecraft:beef", 64, 1), 363, "minecraft:beef");
            itemRegistry.Register(new ItemStack("minecraft:cooked_beef", 64, 1), 364, "minecraft:cooked_beef");
            itemRegistry.Register(new ItemStack("minecraft:chicken", 64, 1), 365, "minecraft:chicken");
            itemRegistry.Register(new ItemStack("minecraft:cooked_chicken", 64, 1), 366, "minecraft:cooked_chicken");
            itemRegistry.Register(new ItemStack("minecraft:rotten_flesh", 64, 1), 367, "minecraft:rotten_flesh");
            itemRegistry.Register(new ItemStack("minecraft:ender_pearl", 16, 1), 368, "minecraft:ender_pearl");
            itemRegistry.Register(new ItemStack("minecraft:blaze_rod", 64, 1), 369, "minecraft:blaze_rod");
            itemRegistry.Register(new ItemStack("minecraft:ghast_tear", 64, 1), 370, "minecraft:ghast_tear");
            itemRegistry.Register(new ItemStack("minecraft:gold_nugget", 64, 1), 371, "minecraft:gold_nugget");
            itemRegistry.Register(new ItemStack("minecraft:nether_wart", 64, 1), 372, "minecraft:nether_wart");
            itemRegistry.Register(new ItemStack("minecraft:potion", 1, 1), 373, "minecraft:potion");
            itemRegistry.Register(new ItemStack("minecraft:glass_bottle", 64, 1), 374, "minecraft:glass_bottle");
            itemRegistry.Register(new ItemStack("minecraft:spider_eye", 64, 1), 375, "minecraft:spider_eye");
            itemRegistry.Register(new ItemStack("minecraft:fermented_spider_eye", 64, 1), 376, "minecraft:fermented_spider_eye");
            itemRegistry.Register(new ItemStack("minecraft:blaze_powder", 64, 1), 377, "minecraft:blaze_powder");
            itemRegistry.Register(new ItemStack("minecraft:magma_cream", 64, 1), 378, "minecraft:magma_cream");
            itemRegistry.Register(new ItemStack("minecraft:brewing_stand", 64, 1), 379, "minecraft:brewing_stand");
            itemRegistry.Register(new ItemStack("minecraft:cauldron", 64, 1), 380, "minecraft:cauldron");
            itemRegistry.Register(new ItemStack("minecraft:ender_eye", 64, 1), 381, "minecraft:ender_eye");
            itemRegistry.Register(new ItemStack("minecraft:speckled_melon", 64, 1), 382, "minecraft:speckled_melon");
            itemRegistry.Register(new ItemStack("minecraft:spawn_egg", 64, 1), 383, "minecraft:spawn_egg");
            itemRegistry.Register(new ItemStack("minecraft:experience_bottle", 64, 1), 384, "minecraft:experience_bottle");
            itemRegistry.Register(new ItemStack("minecraft:fire_charge", 64, 1), 385, "minecraft:fire_charge");
            itemRegistry.Register(new ItemStack("minecraft:writable_book", 1, 1), 386, "minecraft:writable_book");
            itemRegistry.Register(new ItemStack("minecraft:written_book", 16, 1), 387, "minecraft:written_book");
            itemRegistry.Register(new ItemStack("minecraft:emerald", 64, 1), 388, "minecraft:emerald");
            itemRegistry.Register(new ItemStack("minecraft:item_frame", 64, 1), 389, "minecraft:item_frame");
            itemRegistry.Register(new ItemStack("minecraft:flower_pot", 64, 1), 390, "minecraft:flower_pot");
            itemRegistry.Register(new ItemStack("minecraft:carrot", 64, 1), 391, "minecraft:carrot");
            itemRegistry.Register(new ItemStack("minecraft:potato", 64, 1), 392, "minecraft:potato");
            itemRegistry.Register(new ItemStack("minecraft:baked_potato", 64, 1), 393, "minecraft:baked_potato");
            itemRegistry.Register(new ItemStack("minecraft:poisonous_potato", 64, 1), 394, "minecraft:poisonous_potato");
            itemRegistry.Register(new ItemStack("minecraft:map", 64, 1), 395, "minecraft:map");
            itemRegistry.Register(new ItemStack("minecraft:golden_carrot", 64, 1), 396, "minecraft:golden_carrot");
            itemRegistry.Register(new ItemStack("minecraft:skull", 64, 1), 397, "minecraft:skull");
            itemRegistry.Register(new ItemStack("minecraft:carrot_on_a_stick", 1, 1), 398, "minecraft:carrot_on_a_stick");
            itemRegistry.Register(new ItemStack("minecraft:nether_star", 64, 1), 399, "minecraft:nether_star");
            itemRegistry.Register(new ItemStack("minecraft:pumpkin_pie", 64, 1), 400, "minecraft:pumpkin_pie");
            itemRegistry.Register(new ItemStack("minecraft:fireworks", 64, 1), 401, "minecraft:fireworks");
            itemRegistry.Register(new ItemStack("minecraft:firework_charge", 64, 1), 402, "minecraft:firework_charge");
            itemRegistry.Register(new ItemStack("minecraft:enchanted_book", 1, 1), 403, "minecraft:enchanted_book");
            itemRegistry.Register(new ItemStack("minecraft:comparator", 64, 1), 404, "minecraft:comparator");
            itemRegistry.Register(new ItemStack("minecraft:netherbrick", 64, 1), 405, "minecraft:netherbrick");
            itemRegistry.Register(new ItemStack("minecraft:quartz", 64, 1), 406, "minecraft:quartz");
            itemRegistry.Register(new ItemStack("minecraft:tnt_minecart", 1, 1), 407, "minecraft:tnt_minecart");
            itemRegistry.Register(new ItemStack("minecraft:hopper_minecart", 1, 1), 408, "minecraft:hopper_minecart");
            itemRegistry.Register(new ItemStack("minecraft:prismarine_shard", 64, 1), 409, "minecraft:prismarine_shard");
            itemRegistry.Register(new ItemStack("minecraft:prismarine_crystals", 64, 1), 410, "minecraft:prismarine_crystals");
            itemRegistry.Register(new ItemStack("minecraft:rabbit", 64, 1), 411, "minecraft:rabbit");
            itemRegistry.Register(new ItemStack("minecraft:cooked_rabbit", 64, 1), 412, "minecraft:cooked_rabbit");
            itemRegistry.Register(new ItemStack("minecraft:rabbit_stew", 1, 1), 413, "minecraft:rabbit_stew");
            itemRegistry.Register(new ItemStack("minecraft:rabbit_foot", 64, 1), 414, "minecraft:rabbit_foot");
            itemRegistry.Register(new ItemStack("minecraft:rabbit_hide", 64, 1), 415, "minecraft:rabbit_hide");
            itemRegistry.Register(new ItemStack("minecraft:armor_stand", 16, 1), 416, "minecraft:armor_stand");
            itemRegistry.Register(new ItemStack("minecraft:iron_horse_armor", 1, 1), 417, "minecraft:iron_horse_armor");
            itemRegistry.Register(new ItemStack("minecraft:golden_horse_armor", 1, 1), 418, "minecraft:golden_horse_armor");
            itemRegistry.Register(new ItemStack("minecraft:diamond_horse_armor", 1, 1), 419, "minecraft:diamond_horse_armor");
            itemRegistry.Register(new ItemStack("minecraft:lead", 64, 1), 420, "minecraft:lead");
            itemRegistry.Register(new ItemStack("minecraft:name_tag", 64, 1), 421, "minecraft:name_tag");
            itemRegistry.Register(new ItemStack("minecraft:command_block_minecart", 1, 1), 422, "minecraft:command_block_minecart");
            itemRegistry.Register(new ItemStack("minecraft:mutton", 64, 1), 423, "minecraft:mutton");
            itemRegistry.Register(new ItemStack("minecraft:cooked_mutton", 64, 1), 424, "minecraft:cooked_mutton");
            itemRegistry.Register(new ItemStack("minecraft:banner", 16, 1), 425, "minecraft:banner");
            itemRegistry.Register(new ItemStack("minecraft:end_crystal", 64, 1), 426, "minecraft:end_crystal");
            itemRegistry.Register(new ItemStack("minecraft:spruce_door", 64, 1), 427, "minecraft:spruce_door");
            itemRegistry.Register(new ItemStack("minecraft:birch_door", 64, 1), 428, "minecraft:birch_door");
            itemRegistry.Register(new ItemStack("minecraft:jungle_door", 64, 1), 429, "minecraft:jungle_door");
            itemRegistry.Register(new ItemStack("minecraft:acacia_door", 64, 1), 430, "minecraft:acacia_door");
            itemRegistry.Register(new ItemStack("minecraft:dark_oak_door", 64, 1), 431, "minecraft:dark_oak_door");
            itemRegistry.Register(new ItemStack("minecraft:chorus_fruit", 64, 1), 432, "minecraft:chorus_fruit");
            itemRegistry.Register(new ItemStack("minecraft:chorus_fruit_popped", 64, 1), 433, "minecraft:chorus_fruit_popped");
            itemRegistry.Register(new ItemStack("minecraft:beetroot", 64, 1), 434, "minecraft:beetroot");
            itemRegistry.Register(new ItemStack("minecraft:beetroot_seeds", 64, 1), 435, "minecraft:beetroot_seeds");
            itemRegistry.Register(new ItemStack("minecraft:beetroot_soup", 1, 1), 436, "minecraft:beetroot_soup");
            itemRegistry.Register(new ItemStack("minecraft:dragon_breath", 64, 1), 437, "minecraft:dragon_breath");
            itemRegistry.Register(new ItemStack("minecraft:splash_potion", 1, 1), 438, "minecraft:splash_potion");
            itemRegistry.Register(new ItemStack("minecraft:spectral_arrow", 64, 1), 439, "minecraft:spectral_arrow");
            itemRegistry.Register(new ItemStack("minecraft:tipped_arrow", 64, 1), 440, "minecraft:tipped_arrow");
            itemRegistry.Register(new ItemStack("minecraft:lingering_potion", 1, 1), 441, "minecraft:lingering_potion");
            itemRegistry.Register(new ItemStack("minecraft:shield", 1, 1), 442, "minecraft:shield");
            itemRegistry.Register(new ItemStack("minecraft:elytra", 1, 1), 443, "minecraft:elytra");
            itemRegistry.Register(new ItemStack("minecraft:spruce_boat", 1, 1), 444, "minecraft:spruce_boat");
            itemRegistry.Register(new ItemStack("minecraft:birch_boat", 1, 1), 445, "minecraft:birch_boat");
            itemRegistry.Register(new ItemStack("minecraft:jungle_boat", 1, 1), 446, "minecraft:jungle_boat");
            itemRegistry.Register(new ItemStack("minecraft:acacia_boat", 1, 1), 447, "minecraft:acacia_boat");
            itemRegistry.Register(new ItemStack("minecraft:dark_oak_boat", 1, 1), 448, "minecraft:dark_oak_boat");
            itemRegistry.Register(new ItemStack("minecraft:totem_of_undying", 1, 1), 449, "minecraft:totem_of_undying");
            itemRegistry.Register(new ItemStack("minecraft:shulker_shell", 64, 1), 450, "minecraft:shulker_shell");
            itemRegistry.Register(new ItemStack("minecraft:iron_nugget", 64, 1), 452, "minecraft:iron_nugget");
            itemRegistry.Register(new ItemStack("minecraft:record_13", 1, 1), 2256, "minecraft:record_13");
            itemRegistry.Register(new ItemStack("minecraft:record_cat", 1, 1), 2257, "minecraft:record_cat");
            itemRegistry.Register(new ItemStack("minecraft:record_blocks", 1, 1), 2258, "minecraft:record_blocks");
            itemRegistry.Register(new ItemStack("minecraft:record_chirp", 1, 1), 2259, "minecraft:record_chirp");
            itemRegistry.Register(new ItemStack("minecraft:record_far", 1, 1), 2260, "minecraft:record_far");
            itemRegistry.Register(new ItemStack("minecraft:record_mall", 1, 1), 2261, "minecraft:record_mall");
            itemRegistry.Register(new ItemStack("minecraft:record_mellohi", 1, 1), 2262, "minecraft:record_mellohi");
            itemRegistry.Register(new ItemStack("minecraft:record_stal", 1, 1), 2263, "minecraft:record_stal");
            itemRegistry.Register(new ItemStack("minecraft:record_strad", 1, 1), 2264, "minecraft:record_strad");
            itemRegistry.Register(new ItemStack("minecraft:record_ward", 1, 1), 2265, "minecraft:record_ward");
            itemRegistry.Register(new ItemStack("minecraft:record_11", 1, 1), 2266, "minecraft:record_11");
            itemRegistry.Register(new ItemStack("minecraft:record_wait", 1, 1), 2267, "minecraft:record_wait");

            itemRegistry.Register(new HarvestingTool("minecraft:wooden_pickaxe", 60, HarvestTier.Wooden, 2, BlockMaterial.Rock), 270, "minecraft:wooden_pickaxe");
            itemRegistry.Register(new HarvestingTool("minecraft:stone_pickaxe", 132, HarvestTier.Stone, 4, BlockMaterial.Rock), 274, "minecraft:stone_pickaxe");
            itemRegistry.Register(new HarvestingTool("minecraft:iron_pickaxe", 251, HarvestTier.Stone, 6, BlockMaterial.Rock), 257, "minecraft:iron_pickaxe");
            itemRegistry.Register(new HarvestingTool("minecraft:diamond_pickaxe", 1562, HarvestTier.Diamond, 8, BlockMaterial.Rock), 278, "minecraft:diamond_pickaxe");
            itemRegistry.Register(new HarvestingTool("minecraft:golden_pickaxe", 33, HarvestTier.Wooden, 12, BlockMaterial.Rock), 285, "minecraft:golden_pickaxe");

            itemRegistry.Register(new HarvestingTool("minecraft:wooden_shovel", 60, HarvestTier.Wooden, 2, BlockMaterial.Dirt), 269, "minecraft:wooden_shovel");
            itemRegistry.Register(new HarvestingTool("minecraft:stone_shovel", 132, HarvestTier.Stone, 4, BlockMaterial.Dirt), 273, "minecraft:stone_shovel");
            itemRegistry.Register(new HarvestingTool("minecraft:iron_shovel", 251, HarvestTier.Stone, 6, BlockMaterial.Dirt), 256, "minecraft:iron_shovel");
            itemRegistry.Register(new HarvestingTool("minecraft:diamond_shovel", 1562, HarvestTier.Diamond, 8, BlockMaterial.Dirt), 277, "minecraft:diamond_shovel");
            itemRegistry.Register(new HarvestingTool("minecraft:golden_shovel", 33, HarvestTier.Wooden, 12, BlockMaterial.Dirt), 284, "minecraft:golden_shovel");

            itemRegistry.Register(new HarvestingTool("minecraft:wooden_axe", 60, HarvestTier.Wooden, 2, BlockMaterial.Wood), 271, "minecraft:wooden_axe");
            itemRegistry.Register(new HarvestingTool("minecraft:stone_axe", 132, HarvestTier.Stone, 4, BlockMaterial.Wood), 275, "minecraft:stone_axe");
            itemRegistry.Register(new HarvestingTool("minecraft:iron_axe", 251, HarvestTier.Stone, 6, BlockMaterial.Wood), 258, "minecraft:iron_axe");
            itemRegistry.Register(new HarvestingTool("minecraft:diamond_axe", 1562, HarvestTier.Diamond, 8, BlockMaterial.Wood), 279, "minecraft:diamond_axe");
            itemRegistry.Register(new HarvestingTool("minecraft:golden_axe", 33, HarvestTier.Wooden, 12, BlockMaterial.Wood), 286, "minecraft:golden_axe");
        }
#endregion
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
