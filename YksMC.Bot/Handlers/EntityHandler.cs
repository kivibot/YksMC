﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.EntityType;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Bot.Handlers
{
    public class EntityHandler : WorldEventHandler, IWorldEventHandler<SpawnMobPacket>
    {
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

            IEntity entity = new Entity(packet.EntityId, type, location, yaw, pitch, 0);

            return Result(world.ReplaceDimension(
                dimension.ChangeEntity(entity)
            ));
        }
    }
}
