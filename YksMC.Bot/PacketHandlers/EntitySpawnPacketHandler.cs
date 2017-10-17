using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.EntityType;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Bot.PacketHandlers
{
    public class EntitySpawnPacketHandler : WorldEventHandler, IWorldEventHandler<SpawnMobPacket>, IWorldEventHandler<SpawnPlayerPacket>
    {
        private const double _velocityFactor = 1.0 / 8000.0;

        private readonly IEntityTypeRepository _entityTypeRepository;

        public EntitySpawnPacketHandler(IEntityTypeRepository entityTypeRepository)
        {
            _entityTypeRepository = entityTypeRepository;
        }

        public IWorldEventResult Handle(IWorldEvent<SpawnMobPacket> message)
        {
            IWorld world = message.World;
            SpawnMobPacket packet = message.Event;
            IDimension dimension = world.GetCurrentDimension();

            IEntityLocation location = new EntityLocation(packet.Location.X, packet.Location.Y, packet.Location.Z);
            IEntityType type = _entityTypeRepository.GetMobType(packet.Type);
            double yaw = packet.Yaw.GetRadians();
            double pitch = packet.Pitch.GetRadians();

            IVector3<double> velocity = new Vector3d(packet.VelocityX, packet.VelocityY, packet.VelocityZ)
                .Multiply(_velocityFactor);

            IEntity entity = new Entity(packet.EntityId, type, location, yaw, pitch, 0, false, velocity, 1);

            return Result(world.ReplaceDimension(
                dimension.ReplaceEntity(entity)
            ));
        }

        public IWorldEventResult Handle(IWorldEvent<SpawnPlayerPacket> message)
        {
            IWorld world = message.World;
            SpawnPlayerPacket packet = message.Event;

            IEntityType playerEntityType = _entityTypeRepository.GetPlayerType();
            IEntity playerEntity = new Entity(
                packet.EntityId, 
                playerEntityType, 
                new EntityLocation(packet.Position.X, packet.Position.Y, packet.Position.Z), 
                packet.Yaw.GetRadians(), 
                packet.Pitch.GetRadians(), 
                0, 
                false, 
                new Vector3d(0, 0, 0),
                1);

            //TODO: remove
            IPlayer player = new Player(packet.PlayerId, "<Unknown>")
                .ChangeEntity(playerEntity.Id, world.GetCurrentDimension().Id);

            return Result(world.ReplacePlayer(player).ChangeCurrentDimension(dimension => dimension.ReplaceEntity(playerEntity)));
        }
    }
}
