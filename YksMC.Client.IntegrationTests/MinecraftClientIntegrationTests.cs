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
using YksMC.MinecraftModel.Window;
using YksMC.Data.Json.Window;
using YksMC.Data.Json.ItemType;
using YksMC.Bot.GameObjectRegistry;
using YksMC.MinecraftModel.Inventory;
using YksMC.MinecraftModel.ItemStack;

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
            builder.RegisterType<JsonBlockTypeRepository>().AsImplementedInterfaces().SingleInstance();
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

            builder.RegisterType<BehaviorTaskScheduler>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<PathFinder>().AsImplementedInterfaces();
            builder.RegisterType<Random>().SingleInstance();

            IGameObjectRegistry<IInventory> inventoryRegistry = new GameObjectRegistry<IInventory>();
            inventoryRegistry.Register(new PlayerInventory(), "minecraft:player");
            builder.RegisterInstance(inventoryRegistry).As<IGameObjectRegistry<IInventory>>();

            IGameObjectRegistry<IItemStack> itemRegistry = new GameObjectRegistry<IItemStack>();
            RegisterVanillaItems(itemRegistry);
            builder.RegisterInstance(itemRegistry).As<IGameObjectRegistry<IItemStack>>();

            IBlock emptyBlock = new Block(new BlockType("air", false, false, 0, HarvestTier.Hand), new LightLevel(0), new LightLevel(0), new Biome("void"));
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

        private static void RegisterVanillaItems(IGameObjectRegistry<IItemStack> itemRegistry)
        {
            itemRegistry.Register(new ItemStack("minecraft:iron_shovel", 1, 1), 256, "minecraft:iron_shovel");
            itemRegistry.Register(new ItemStack("minecraft:iron_pickaxe", 1, 1), 257, "minecraft:iron_pickaxe");
            itemRegistry.Register(new ItemStack("minecraft:iron_axe", 1, 1), 258, "minecraft:iron_axe");
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
            itemRegistry.Register(new ItemStack("minecraft:wooden_shovel", 1, 1), 269, "minecraft:wooden_shovel");
            itemRegistry.Register(new ItemStack("minecraft:wooden_pickaxe", 1, 1), 270, "minecraft:wooden_pickaxe");
            itemRegistry.Register(new ItemStack("minecraft:wooden_axe", 1, 1), 271, "minecraft:wooden_axe");
            itemRegistry.Register(new ItemStack("minecraft:stone_sword", 1, 1), 272, "minecraft:stone_sword");
            itemRegistry.Register(new ItemStack("minecraft:stone_shovel", 1, 1), 273, "minecraft:stone_shovel");
            itemRegistry.Register(new ItemStack("minecraft:stone_pickaxe", 1, 1), 274, "minecraft:stone_pickaxe");
            itemRegistry.Register(new ItemStack("minecraft:stone_axe", 1, 1), 275, "minecraft:stone_axe");
            itemRegistry.Register(new ItemStack("minecraft:diamond_sword", 1, 1), 276, "minecraft:diamond_sword");
            itemRegistry.Register(new ItemStack("minecraft:diamond_shovel", 1, 1), 277, "minecraft:diamond_shovel");
            itemRegistry.Register(new ItemStack("minecraft:diamond_pickaxe", 1, 1), 278, "minecraft:diamond_pickaxe");
            itemRegistry.Register(new ItemStack("minecraft:diamond_axe", 1, 1), 279, "minecraft:diamond_axe");
            itemRegistry.Register(new ItemStack("minecraft:stick", 64, 1), 280, "minecraft:stick");
            itemRegistry.Register(new ItemStack("minecraft:bowl", 64, 1), 281, "minecraft:bowl");
            itemRegistry.Register(new ItemStack("minecraft:mushroom_stew", 1, 1), 282, "minecraft:mushroom_stew");
            itemRegistry.Register(new ItemStack("minecraft:golden_sword", 1, 1), 283, "minecraft:golden_sword");
            itemRegistry.Register(new ItemStack("minecraft:golden_shovel", 1, 1), 284, "minecraft:golden_shovel");
            itemRegistry.Register(new ItemStack("minecraft:golden_pickaxe", 1, 1), 285, "minecraft:golden_pickaxe");
            itemRegistry.Register(new ItemStack("minecraft:golden_axe", 1, 1), 286, "minecraft:golden_axe");
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
                new BreakBlockCommand(new BlockLocation(2676, 4, -796)),
                new IUrgeScorer[] {
                    new ConstantScorer(0.5)
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
