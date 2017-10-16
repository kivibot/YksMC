using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Behavior.Task;
using YksMC.Behavior.Urge;
using YksMC.Bot.PacketHandlers;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.EventBus.Bus;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Bot.Core
{
    public class TaskLoop
    {
        private readonly ILogger _logger;
        private readonly IUrgeManager _urgeManager;
        private readonly IBehaviorTaskManager _behaviorTaskManager;
        private readonly IMinecraftClient _minecraftClient;
        private readonly IEventBus _eventBus;
        private readonly ConcurrentQueue<object> _packetQueue;

        private IBehaviorTask _task;
        private IWorld _world;

        public TaskLoop(ILogger logger, IUrgeManager urgeManager, IBehaviorTaskManager behaviorTaskManager,
            IMinecraftClient minecraftClient, IEventBus eventBus, IWorld world)
        {
            _logger = logger;
            _urgeManager = urgeManager;
            _behaviorTaskManager = behaviorTaskManager;
            _minecraftClient = minecraftClient;
            _eventBus = eventBus;
            _world = world;

            _packetQueue = new ConcurrentQueue<object>();
            _minecraftClient.PacketReceived += _packetQueue.Enqueue;
        }

        public async Task LoopAsync()
        {
            while (true)
            {
                LoopOnce();
                await Task.Delay(50);
            }
        }

        private void LoopOnce()
        {
            if (_task == null || _task.IsCompleted)
            {
                GetNextTask();
            }

            HandlePackets();

            if (_task != null)
            {
                _world = _task.OnTick(_world);
            }

            SendChanges();
        }

        private void SendChanges()
        {
            IPlayer player = _world.GetLocalPlayer();
            if (!player.HasEntity)
            {
                return;
            }
            IEntity entity = _world.GetCurrentDimension().GetEntity(player.EntityId);
            _minecraftClient.SendPacket(new PlayerPositionAndLookPacket()
            {
                X = entity.Location.X,
                FeetY = entity.Location.Y,
                Z = entity.Location.Z,
                Yaw = (float)(entity.Yaw / Math.PI * 180.0),
                Pitch = (float)(entity.Pitch / Math.PI * 180.0),
                OnGround = entity.IsOnGround
            });
        }

        private void GetNextTask()
        {
            _logger.Information("Scheduling new task.");
            IUrge urge = _urgeManager.GetLargestUrge(_world);
            if (urge == null)
            {
                return;
            }
            _logger.Information("Largest urge: {Name}, {Score}", urge.Name, urge.GetScore(_world));
            _task = _behaviorTaskManager.GetTask(urge.TaskName);
            _world = _task.OnStart(_world);
        }

        private void HandlePackets()
        {
            while (_packetQueue.TryDequeue(out object packet))
            {
                HandlePacket((dynamic)packet);
            }
        }

        private void HandlePacket<T>(T packet)
        {
            IReadOnlyList<IWorldEventResult> results = _eventBus.Handle<IWorldEvent<T>, IWorldEventResult>(new WorldEvent<T>(_world, packet));
            foreach (IWorldEventResult result in results)
            {
                HandleWorldEventResult(result);
            }
            _task?.OnPacketReceived(packet);
        }

        private void HandleWorldEventResult(IWorldEventResult result)
        {
            foreach (object replyPacket in result.ReplyPackets)
            {
                _minecraftClient.SendPacket(replyPacket);
            }
            _world = result.World;
        }
    }
}
