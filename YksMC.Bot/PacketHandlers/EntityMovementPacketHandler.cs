using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Bot.PacketHandlers
{
    public class EntityMovementPacketHandler : WorldEventHandler, IWorldEventHandler<EntityRelativeMovePacket>,
        IWorldEventHandler<EntityLookAndRelativeMovePacket>, IWorldEventHandler<EntityTeleportPacket>,
        IWorldEventHandler<EntityLookPacket>
    {
        private const double _relativeMovementFactor = 1.0 / 4096.0;

        private readonly ILogger _logger;

        public EntityMovementPacketHandler(ILogger logger)
        {
            _logger = logger;
        }

        public IWorldEventResult Handle(IWorldEvent<EntityRelativeMovePacket> message)
        {
            EntityRelativeMovePacket packet = message.Event;
            IWorld world = message.World;

            IDimension dimension = world.GetCurrentDimension();
            IEntity entity;
            if (!dimension.Entities.TryGetEntity(packet.EntityId, out entity))
            {
                _logger.Warning("Unknown entity: {Id}", packet.EntityId);
                return Result(message);
            }

            IEntityLocation location = entity.Location.Add(new Vector3d(
                packet.DeltaPosition.X * _relativeMovementFactor,
                packet.DeltaPosition.Y * _relativeMovementFactor,
                packet.DeltaPosition.Z * _relativeMovementFactor));

            entity = entity.ChangeLocation(location)
                .ChangeOnGround(packet.IsOnGround);
            dimension = dimension.ReplaceEntity(entity);
            world = world.ReplaceCurrentDimension(dimension);
            return Result(world);
        }

        public IWorldEventResult Handle(IWorldEvent<EntityLookAndRelativeMovePacket> message)
        {
            EntityLookAndRelativeMovePacket packet = message.Event;
            IWorld world = message.World;

            IDimension dimension = world.GetCurrentDimension();
            IEntity entity;
            if (!dimension.Entities.TryGetEntity(packet.EntityId, out entity))
            {
                _logger.Warning("Unknown entity: {Id}", packet.EntityId);
                return Result(message);
            }

            IEntityLocation location = entity.Location.Add(new Vector3d(
                packet.DeltaPosition.X * _relativeMovementFactor,
                packet.DeltaPosition.Y * _relativeMovementFactor,
                packet.DeltaPosition.Z * _relativeMovementFactor));

            entity = entity.ChangeLocation(location)
                .ChangeLook(packet.Yaw.GetRadians(), packet.Pitch.GetRadians())
                .ChangeOnGround(packet.IsOnGround);
            dimension = dimension.ReplaceEntity(entity);
            world = world.ReplaceCurrentDimension(dimension);
            return Result(world);
        }

        public IWorldEventResult Handle(IWorldEvent<EntityTeleportPacket> message)
        {
            EntityTeleportPacket packet = message.Event;
            IWorld world = message.World;

            IDimension dimension = world.GetCurrentDimension();
            IEntity entity;
            if (!dimension.Entities.TryGetEntity(packet.EntityId, out entity))
            {
                _logger.Warning("Unknown entity: {Id}", packet.EntityId);
                return Result(message);
            }

            entity = entity.ChangeLocation(new EntityLocation(packet.Position.X, packet.Position.Y, packet.Position.Z))
                .ChangeLook(packet.Yaw.GetRadians(), packet.Pitch.GetRadians())
                .ChangeOnGround(packet.IsOnGround);
            dimension = dimension.ReplaceEntity(entity);
            world = world.ReplaceCurrentDimension(dimension);
            return Result(world);
        }

        public IWorldEventResult Handle(IWorldEvent<EntityLookPacket> message)
        {
            EntityLookPacket packet = message.Event;
            IWorld world = message.World;

            IDimension dimension = world.GetCurrentDimension();
            IEntity entity;
            if (!dimension.Entities.TryGetEntity(packet.EntityId, out entity))
            {
                _logger.Warning("Unknown entity: {Id}", packet.EntityId);
                return Result(message);
            }

            entity = entity.ChangeLook(packet.Yaw.GetRadians(), packet.Pitch.GetRadians())
                .ChangeOnGround(packet.IsOnGround);
            dimension = dimension.ReplaceEntity(entity);
            world = world.ReplaceCurrentDimension(dimension);
            return Result(world);
        }
    }
}
