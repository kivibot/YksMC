using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using YksMC.Behavior.Misc;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.TickHandlers
{
    public class PlayerGravityHandler : WorldEventHandler, IWorldEventHandler<IGameTick>
    {
        private const double _acceleration = -0.08;
        private const double _drag = 0.02;
        private const double _dragFactor = 1.0 - _drag;
        private const double _maxVelocity = 3.92;

        private readonly IPlayerCollisionDetectionService _playerCollisionDetectionService;

        public PlayerGravityHandler(IPlayerCollisionDetectionService playerCollisionDetectionService)
        {
            _playerCollisionDetectionService = playerCollisionDetectionService;
        }

        public IWorldEventResult Handle(IWorldEvent<IGameTick> message)
        {
            IWorld world = message.World;
            IGameTick tick = message.Event;
            IPlayer player = world.GetLocalPlayer();
            if (player == null || !player.HasEntity)
            {
                return Result(world);
            }
            IDimension dimension = world.GetCurrentDimension();
            IEntity entity = dimension.Entities[player.EntityId];

            IVector3<double> velocity = GetNextVelocity(entity.Velocity);

            entity = _playerCollisionDetectionService.UpdatePlayerPosition(world, velocity);
            if (entity.IsOnGround)
            {
                velocity = Vector3d.Zero;
            }
            entity = entity.ChangeVelocity(velocity);

            return Result(world.ChangeCurrentDimension(d => d.ReplaceEntity(entity)));
        }
               
        private IVector3<double> GetNextVelocity(IVector3<double> previousVelocity)
        {
            IVector3<double> velocity = new Vector3d(0, previousVelocity.Y * _dragFactor + _acceleration, 0);
            return velocity;
        }
    }
}
