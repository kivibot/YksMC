using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.EntityType;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.Dimension;
using YksMC.Protocol.Packets.Play.Clientbound;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Bot.Handlers
{
    public class PlayerHandler : IWorldEventHandler<JoinGamePacket>, IWorldEventHandler<PlayerPositionLookPacket>
    {
        private readonly IEntityTypeRepository _entityTypeRepository;

        public PlayerHandler(IEntityTypeRepository entityTypeRepository)
        {
            _entityTypeRepository = entityTypeRepository;
        }

        public IDimension ApplyEvent(JoinGamePacket packet, IDimension dimension)
        {
            IEntityType playerEntityType = _entityTypeRepository.GetPlayerType();
            IEntity playerEntity = new Entity(packet.EntityId, playerEntityType, EntityCoordinate.Origin);

            IPlayer player = dimension.GetLocalPlayer();
            player = player.ChangeEntity(playerEntity.Id);

            return dimension.ChangeEntity(playerEntity)
                .ReplacePlayer(player);
        }

        public IDimension ApplyEvent(PlayerPositionLookPacket packet, IDimension dimension)
        {
            PlayerPositionLookPacketFlags flags = packet.Flags;
            IEntity playerEntity = dimension.GetLocalPlayerEntity();

            double x = (flags.RelativeX ? playerEntity.Position.X : 0) + packet.X;
            double y = (flags.RelativeY ? playerEntity.Position.Y : 0) + packet.FeetY;
            double z = (flags.RelativeZ ? playerEntity.Position.Z : 0) + packet.Z;
            playerEntity = playerEntity.ChangePosition(new EntityCoordinate(x, y, z));

            TeleportConfirmPacket confirmationPacket = new TeleportConfirmPacket()
            {
                TeleportId = packet.TeleportId
            };

            return dimension.ChangeEntity(playerEntity);
        }
    }
}
