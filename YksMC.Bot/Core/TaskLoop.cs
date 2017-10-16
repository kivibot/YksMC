using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using YksMC.Behavior.Task;
using YksMC.Behavior.Urge;
using YksMC.Bot.PacketHandlers;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.Core
{
    public class TaskLoop
    {
        private readonly ILogger _logger;
        private readonly IUrgeManager _urgeManager;
        private readonly IBehaviorTaskManager _behaviorTaskManager;
        private readonly IMinecraftClient _minecraftClient;
        private readonly WorldEventHandlerWrapper _worldEventHandlerWrapper;
        private readonly ConcurrentQueue<object> _packetQueue;

        private IBehaviorTask _task;
        private IWorld _world;

        public TaskLoop(ILogger logger, IUrgeManager urgeManager, IBehaviorTaskManager behaviorTaskManager, 
            IMinecraftClient minecraftClient, WorldEventHandlerWrapper worldEventHandlerWrapper, IWorld world)
        {
            _logger = logger;
            _urgeManager = urgeManager;
            _behaviorTaskManager = behaviorTaskManager;
            _minecraftClient = minecraftClient;
            _worldEventHandlerWrapper = worldEventHandlerWrapper;
            _world = world;

            _packetQueue = new ConcurrentQueue<object>();
            _minecraftClient.PacketReceived += _packetQueue.Enqueue;
        }

        public void Run()
        {
            //TODO: better condition
            while (true)
            {
                LoopOnce();
            }
        }

        private void LoopOnce()
        {
            if(_task == null || _task.IsCompleted)
            {
                _logger.Information("Scheduling new task.");
                IUrge urge = _urgeManager.GetLargestUrge(_world);
                _logger.Information("Largest urge: {Name}, {Score}", urge.Name, urge.GetScore(_world));
                _task = _behaviorTaskManager.GetTask(urge.TaskName);
                _world = _task.OnStart(_world);
            }

            HandlePackets();

            if(_task != null)
            {
                _world = _task.OnTick(_world);
            }
        }

        private void HandlePackets()
        {
            while (_packetQueue.TryDequeue(out object packet))
            {
                IWorldEventResult result = _worldEventHandlerWrapper.ApplyEvent(packet, _world);
                foreach (object replyPacket in result.ReplyPackets)
                {
                    _minecraftClient.SendPacket(replyPacket);
                }
                _world = result.World;
                _task?.OnPacketReceived(packet);
            }
        }


    }
}
