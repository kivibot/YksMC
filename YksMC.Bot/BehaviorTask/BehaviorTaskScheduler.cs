using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.BehaviorTask
{
    public class BehaviorTaskScheduler : IBehaviorTaskScheduler
    {
        private ConcurrentQueue<Tuple<IBehaviorTask, TaskCompletionSource<bool>>> _enqueuedTasks;
        private IList<Tuple<IBehaviorTask, TaskCompletionSource<bool>>> _runningTasks;

        public BehaviorTaskScheduler()
        {
            _enqueuedTasks = new ConcurrentQueue<Tuple<IBehaviorTask, TaskCompletionSource<bool>>>();
            _runningTasks = new List<Tuple<IBehaviorTask, TaskCompletionSource<bool>>>();
        }

        public void EnqueueTask(IBehaviorTask task)
        {
            _enqueuedTasks.Enqueue(new Tuple<IBehaviorTask, TaskCompletionSource<bool>>(task, null));
        }

        public IWorldEventResult HandleTick(IWorld world, IGameTick tick)
        {
            List<object> replyPackets = new List<object>();
            IList<Tuple<IBehaviorTask, TaskCompletionSource<bool>>> runningTasks = new List<Tuple<IBehaviorTask, TaskCompletionSource<bool>>>();

            while (_enqueuedTasks.TryDequeue(out Tuple<IBehaviorTask, TaskCompletionSource<bool>> task))
            {
                runningTasks.Add(task);
                IWorldEventResult result = task.Item1.OnStart(world);
                world = result.World;
                replyPackets.AddRange(result.ReplyPackets);
            }

            foreach(Tuple<IBehaviorTask, TaskCompletionSource<bool>> task in _runningTasks)
            {
                if (task.Item1.IsCompleted)
                {
                    task.Item2?.SetResult(true);
                    continue;
                }
                runningTasks.Add(task);
                task.Item1.OnTick(world, tick);
            }
            _runningTasks = runningTasks;

            return new WorldEventResult(world, replyPackets);
        }

        public async Task<IBehaviorTask> RunTaskAsync(IBehaviorTask task)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            _enqueuedTasks.Enqueue(new Tuple<IBehaviorTask, TaskCompletionSource<bool>>(task, tcs));
            await tcs.Task;
            return task;
        }
    }
}
