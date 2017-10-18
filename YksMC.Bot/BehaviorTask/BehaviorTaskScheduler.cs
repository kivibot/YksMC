using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.BehaviorTask
{
    public class BehaviorTaskScheduler : IBehaviorTaskScheduler
    {
        private readonly IBehaviorTaskManager _taskManager;

        private ConcurrentQueue<Tuple<IBehaviorTask, TaskCompletionSource<bool>>> _pendingTasks;
        private IList<Tuple<IBehaviorTask, TaskCompletionSource<bool>>> _runningTasks;

        public BehaviorTaskScheduler(IBehaviorTaskManager taskManager)
        {
            _pendingTasks = new ConcurrentQueue<Tuple<IBehaviorTask, TaskCompletionSource<bool>>>();
            _runningTasks = new List<Tuple<IBehaviorTask, TaskCompletionSource<bool>>>();
            _taskManager = taskManager;
        }

        public void EnqueueTask(IBehaviorTask task)
        {
            _pendingTasks.Enqueue(new Tuple<IBehaviorTask, TaskCompletionSource<bool>>(task, null));
        }

        public IBehaviorTask EnqueueTask(object command)
        {
            IBehaviorTask task = _taskManager.GetTask(command);
            EnqueueTask(task);
            return task;
        }

        public IWorldEventResult HandleTick(IWorld world, IGameTick tick)
        {
            List<object> replyPackets = new List<object>();

            List<Tuple<IBehaviorTask, TaskCompletionSource<bool>>> runningTasks = TickExistingTasks(world, tick, _runningTasks).ToList();

            do
            {
                List<Tuple<IBehaviorTask, TaskCompletionSource<bool>>> newTasks = new List<Tuple<IBehaviorTask, TaskCompletionSource<bool>>>();
                world = StartPendingTasks(world, replyPackets, newTasks);
                runningTasks.AddRange(TickExistingTasks(world, tick, newTasks));
            } while (!_pendingTasks.IsEmpty);

            _runningTasks = runningTasks;

            return new WorldEventResult(world, replyPackets);
        }

        private IEnumerable<Tuple<IBehaviorTask, TaskCompletionSource<bool>>> TickExistingTasks(IWorld world, IGameTick tick, IList<Tuple<IBehaviorTask, TaskCompletionSource<bool>>> runningTasks)
        {
            foreach (Tuple<IBehaviorTask, TaskCompletionSource<bool>> task in runningTasks)
            {
                if (task.Item1.IsCompleted)
                {
                    task.Item2?.TrySetResult(true);
                    continue;
                }
                task.Item1.OnTick(world, tick);
                yield return task;
            }
        }

        private IWorld StartPendingTasks(IWorld world, List<object> replyPackets, IList<Tuple<IBehaviorTask, TaskCompletionSource<bool>>> newTasks)
        {
            while (_pendingTasks.TryDequeue(out Tuple<IBehaviorTask, TaskCompletionSource<bool>> task))
            {
                newTasks.Add(task);
                IWorldEventResult result = task.Item1.OnStart(world);
                world = result.World;
                replyPackets.AddRange(result.ReplyPackets);
            }

            return world;
        }

        public async Task RunTaskAsync(IBehaviorTask task)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            _pendingTasks.Enqueue(new Tuple<IBehaviorTask, TaskCompletionSource<bool>>(task, tcs));
            await tcs.Task;
        }

        public async Task<IBehaviorTask> RunTaskAsync(object command)
        {
            IBehaviorTask task = _taskManager.GetTask(command);
            await RunTaskAsync(task);
            return task;
        }

    }
}
