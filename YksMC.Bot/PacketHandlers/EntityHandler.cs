using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.EntityType;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Bot.PacketHandlers
{
    public class EntityHandler : WorldEventHandler, IWorldEventHandler<SpawnMobPacket>, IWorldEventHandler<EntityRelativeMovePacket>, IWorldEventHandler<EntityLookAndRelativeMovePacket>
    {
        private const double _velocityFactor = 1.0 / 8000.0;

        private const double _relativeMoveFactor = 1.0 / 4096.0;

        private readonly IEntityTypeRepository _entityTypeRepository;

        public EntityHandler(IEntityTypeRepository entityTypeRepository)
        {
            _entityTypeRepository = entityTypeRepository;
        }

        public IWorldEventResult ApplyEvent(SpawnMobPacket packet, IWorld world)
        {
            IDimension dimension = world.GetCurrentDimension();

            IEntityLocation location = new EntityLocation(packet.Location.X, packet.Location.Y, packet.Location.Z);
            IEntityType type = _entityTypeRepository.GetMobType(packet.Type);
            double yaw = packet.Yaw.GetRadians();
            double pitch = packet.Pitch.GetRadians();

            IVector3<double> velocity = new Vector3d(packet.VelocityX, packet.VelocityY, packet.VelocityZ)
                .Multiply(_velocityFactor);

            IEntity entity = new Entity(packet.EntityId, type, location, yaw, pitch, 0, false, velocity);

            return Result(world.ReplaceDimension(
                dimension.ChangeEntity(entity)
            ));
        }

        public IWorldEventResult ApplyEvent(EntityRelativeMovePacket packet, IWorld world)
        {
            IDimension dimension = world.GetCurrentDimension();

            try
            {
                IEntity entity = dimension.GetEntity(packet.EntityId);
                entity = entity.ChangeLocation(entity.Location.Add(new Vector3d(packet.DeltaPosition.X * _relativeMoveFactor, packet.DeltaPosition.Y * _relativeMoveFactor, packet.DeltaPosition.Z * _relativeMoveFactor)));

                return Result(world.ReplaceCurrentDimension(dimension.ChangeEntity(entity)));
            }
            catch (Exception ex) { return Result(world); }
        }

        public IWorldEventResult ApplyEvent(EntityLookAndRelativeMovePacket packet, IWorld world)
        {
            IDimension dimension = world.GetCurrentDimension();

            try
            {
                IEntity entity = dimension.GetEntity(packet.EntityId);
                entity = entity.ChangeLocation(entity.Location.Add(new Vector3d(packet.DeltaPosition.X * _relativeMoveFactor, packet.DeltaPosition.Y * _relativeMoveFactor, packet.DeltaPosition.Z * _relativeMoveFactor)));

                return Result(world.ReplaceCurrentDimension(dimension.ChangeEntity(entity)));
            }
            catch (Exception ex) { return Result(world); }
        }
    }
}
