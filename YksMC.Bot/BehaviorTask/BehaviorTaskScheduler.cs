using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.BehaviorTask
{
    public class BehaviorTaskScheduler : IBehaviorTaskScheduler
    {
        private ConcurrentQueue<IBehaviorTask> _enqueuedTasks;
        private IList<IBehaviorTask> _runningTasks;

        public BehaviorTaskScheduler()
        {
            _enqueuedTasks = new ConcurrentQueue<IBehaviorTask>();
            _runningTasks = new List<IBehaviorTask>();
        }

        public void EnqueueTask(IBehaviorTask task)
        {
            _enqueuedTasks.Enqueue(task);
        }

        public IWorldEventResult HandleTick(IWorld world, IGameTick tick)
        {
            List<object> replyPackets = new List<object>();
            IList<IBehaviorTask> runningTasks = new List<IBehaviorTask>();

            while (_enqueuedTasks.TryDequeue(out IBehaviorTask task))
            {
                runningTasks.Add(task);
                IWorldEventResult result = task.OnStart(world);
                world = result.World;
                replyPackets.AddRange(result.ReplyPackets);
            }

            foreach(IBehaviorTask task in _runningTasks)
            {
                if (task.IsCompleted)
                {
                    continue;
                }
                runningTasks.Add(task);
                task.OnTick(world, tick);
            }
            _runningTasks = runningTasks;

            return new WorldEventResult(world, replyPackets);
        }
    }
}
