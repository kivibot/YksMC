using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.BehaviorTask;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Tasks.Movement
{
    public class MoveToLocationTask : BehaviorTask<MoveToLocationCommand>
    {
        private const double _targetYOffset = 0.02;
        private const double _velocity = 0.2;
        private const double _timeoutFactor = 2 / _velocity;

        public override string Name => $"MoveToLocation({_command.Location.X}, {_command.Location.Y}, {_command.Location.Z})";

        private Task _task;
        private int _tickCount;
        private int _timeout;

        public MoveToLocationTask(MoveToLocationCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler)
            : base(command, minecraftClient, taskScheduler)
        {
            _tickCount = 0;
        }

        public override bool IsPossible(IWorld world)
        {
            return true;
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {
            _timeout = (int)(_timeoutFactor * _command.Location.AsVector()
                .Substract(world.GetPlayerEntity().Location.AsVector())
                .Length());
            return Result(world);
        }
        
        public override IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick)
        {
            IVector3<double> delta = _command.Location.AsVector()
                .Add(new Vector3d(0, _targetYOffset, 0))
                .Substract(world.GetPlayerEntity().Location.AsVector());
            if (delta.Length() <= _command.MinimumDistance)
            {
                return Success(world);
            }
            if (_tickCount > _timeout)
            {
                return Failure(world);
            }
            _tickCount++;
            if (_task != null && !_task.IsCompleted)
            {
                return Result(world);
            }
            delta = new Vector3d(delta.X, 0, delta.Z);
            if (delta.Length() == 0)
            {
                return Success(world);
            }
            IVector3<double> nextMovement = delta.Normalize().Multiply(Math.Min(delta.Length(), _velocity));
            _task = _taskScheduler.RunCommandAsync(new MoveLinearCommand(nextMovement));
            _taskScheduler.EnqueueCommand(new LookAtCommand(_command.Location));
            return Result(world);
        }
    }
}
