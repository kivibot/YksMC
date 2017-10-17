using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks.Movement
{
    public class MoveToLocationTask : BehaviorTask<MoveToLocationCommand>
    {
        private const double _targetYOffset = 0.02;
        private const double _velocity = 0.2;
        private const double _timeoutFactor = 2 / _velocity;

        private readonly IBehaviorTaskScheduler _taskScheduler;

        public override string Name => $"MoveToLocation({_command.Location.X}, {_command.Location.Y}, {_command.Location.Z})";

        private IBehaviorTask _task;
        private int _tickCount;
        private int _timeout;

        public MoveToLocationTask(MoveToLocationCommand command, IBehaviorTaskScheduler taskScheduler)
            : base(command)
        {
            _taskScheduler = taskScheduler;
            _tickCount = 0;
        }

        public override IWorldEventResult OnStart(IWorld world)
        {
            _timeout = (int)(_timeoutFactor * _command.Location.AsVector()
                .Substract(world.GetPlayerEntity().Location.AsVector())
                .Length());
            return Result(world);
        }


        public override void OnTick(IWorld world, IGameTick tick)
        {
            IVector3<double> delta = _command.Location.AsVector()
                .Add(new Vector3d(0, _targetYOffset, 0))
                .Substract(world.GetPlayerEntity().Location.AsVector());
            if (delta.Length() <= _command.MinimumDistance)
            {
                Complete();
                return;
            }
            if(_tickCount > _timeout)
            {
                Fail();
                return;
            }
            _tickCount++;
            if (_task != null && !_task.IsCompleted)
            {
                return;
            }
            IVector3<double> nextMovement = delta.Normalize().Multiply(Math.Min(delta.Length(), _velocity));
            _task = _taskScheduler.EnqueueTask(new MoveLinearCommand(nextMovement));
            return;
        }
    }
}
