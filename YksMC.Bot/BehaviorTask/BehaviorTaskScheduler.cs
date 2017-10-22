using Serilog;
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
        private readonly ILogger _logger;

        private readonly ConcurrentQueue<TaskPair> _pendingTasks;
        private readonly SortedDictionary<BehaviorTaskPriority, IList<TaskPair>> _tasks;

        public BehaviorTaskScheduler(IBehaviorTaskManager taskManager, ILogger logger)
        {
            _taskManager = taskManager;
            _pendingTasks = new ConcurrentQueue<TaskPair>();
            _tasks = new SortedDictionary<BehaviorTaskPriority, IList<TaskPair>>();
            _logger = logger;
        }

        public async void EnqueueCommand(object command)
        {
            await RunCommandAsync(command);
        }

        public IWorld HandleTick(IWorld world, IGameTick tick)
        {
            world = HandlePending(world);
            world = HandleRunning(world, tick);
            return world;
        }

        private IWorld HandlePending(IWorld world)
        {
            while (_pendingTasks.TryDequeue(out TaskPair pair))
            {
                if (pair.TaskCompletionSource.Task.IsCompleted)
                {
                    continue;
                }
                IBehaviorTaskEventResult result = pair.Task.OnStart(world);
                world = result.World;
                if (result.IsCompleted)
                {
                    pair.TaskCompletionSource.TrySetResult(!result.IsFailed);
                    continue;
                }
                AddTaskToDictionary(pair);
                HandleSinglePendingAsync(pair, world);
            }
            return world;
        }

        private IWorld HandleRunning(IWorld world, IGameTick tick)
        {
            IEnumerable<IList<TaskPair>> lists = _tasks.Values.ToList();
            _tasks.Clear();

            foreach (IList<TaskPair> list in lists)
            {
                world = HandleRunning(list, world, tick);
            }

            return world;
        }

        private IWorld HandleRunning(IList<TaskPair> tasks, IWorld world, IGameTick tick)
        {
            foreach (TaskPair pair in tasks)
            {
                if(pair.TaskCompletionSource.Task.IsCompleted)
                {
                    continue;
                }
                IBehaviorTaskEventResult result = pair.Task.OnTick(world, tick);
                world = result.World;
                if (result.IsCompleted)
                {
                    pair.TaskCompletionSource.TrySetResult(!result.IsFailed);
                    continue;
                }
                AddTaskToDictionary(pair);
            }
            return world;
        }

        private async void HandleSinglePendingAsync(TaskPair pair, IWorld world)
        {
            await Task.Yield();
            bool? success = await pair.Task.OnStartAsync(world);
            if (!success.HasValue)
            {
                return;
            }
            pair.TaskCompletionSource.TrySetResult(success.Value);
        }

        private void AddTaskToDictionary(TaskPair pair)
        {
            if (!_tasks.TryGetValue(pair.Task.Priority, out IList<TaskPair> taskList))
            {
                taskList = new List<TaskPair>();
                _tasks[pair.Task.Priority] = taskList;
            }
            taskList.Add(pair);
        }

        public async Task<bool> RunCommandAsync(object command)
        {
            IBehaviorTask task = _taskManager.GetTask(command);

            _logger.Debug("Task queued: {Name}", task.Name);

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            _pendingTasks.Enqueue(new TaskPair(task, tcs));

            bool success = await tcs.Task;

            _logger.Debug("Task completed: {Name}, Success: {Success}", task.Name, success);

            return success;
        }

        private class TaskPair
        {
            public IBehaviorTask Task { get; }
            public TaskCompletionSource<bool> TaskCompletionSource { get; }

            public TaskPair(IBehaviorTask task, TaskCompletionSource<bool> taskCompletionSource)
            {
                Task = task;
                TaskCompletionSource = taskCompletionSource;
            }
        }
    }
}
