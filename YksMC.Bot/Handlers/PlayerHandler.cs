using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.EntityType;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.Dimension;
using YksMC.Protocol.Packets.Play.Clientbound;
using YksMC.Protocol.Packets.Play.Serverbound;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.Handlers
{
    public class PlayerHandler : IWorldEventHandler<JoinGamePacket>, IWorldEventHandler<PlayerPositionLookPacket>,
        IWorldEventHandler<SetExperiencePacket>
    {
        private readonly IEntityTypeRepository _entityTypeRepository;

        public PlayerHandler(IEntityTypeRepository entityTypeRepository)
        {
            _entityTypeRepository = entityTypeRepository;
        }

        public IWorld ApplyEvent(JoinGamePacket packet, IWorld world)
        {
            IEntityType playerEntityType = _entityTypeRepository.GetPlayerType();
            IEntity playerEntity = new Entity(packet.EntityId, playerEntityType, EntityCoordinate.Origin, 0, 0, 0);

            IDimension dimension = world.GetDimension(packet.Dimension)
                .ChangeEntity(playerEntity);

            IPlayer player = world.GetLocalPlayer()
                .ChangeEntity(playerEntity.Id, dimension.Id);

            return world.ReplaceCurrentDimension(dimension)
                .ReplacePlayer(player);
        }

        public IWorld ApplyEvent(PlayerPositionLookPacket packet, IWorld world)
        {
            IDimension dimension = world.GetCurrentDimension();
            if(dimension == null)
            {
                throw new ArgumentException("No current dimension!");
            }
            IPlayer player = world.GetLocalPlayer();
            if(player == null)
            {
                throw new ArgumentException("Local player not initialized.");
            }
            if (!player.HasEntity)
            {
                throw new ArgumentException("Local player not spawned.");
            }
            IEntity playerEntity = dimension.GetEntity(player.EntityId);

            PlayerPositionLookPacketFlags flags = packet.Flags;
            double x = (flags.RelativeX ? playerEntity.Position.X : 0) + packet.X;
            double y = (flags.RelativeY ? playerEntity.Position.Y : 0) + packet.FeetY;
            double z = (flags.RelativeZ ? playerEntity.Position.Z : 0) + packet.Z;
            double yaw = (flags.RelativeYaw ? playerEntity.Yaw : 0) + packet.Yaw;
            double pitch = (flags.RelativePitch ? playerEntity.Pitch : 0) + packet.Pitch;

            playerEntity = playerEntity.ChangePosition(new EntityCoordinate(x, y, z))
                .ChangeLook(yaw, pitch);

            TeleportConfirmPacket confirmationPacket = new TeleportConfirmPacket()
            {
                TeleportId = packet.TeleportId
            };

            return world.ReplaceCurrentDimension(dimension.ChangeEntity(playerEntity));
        }

        public IWorld ApplyEvent(SetExperiencePacket packet, IWorld world)
        {
            IPlayer player = world.GetLocalPlayer();
            if (player == null)
            {
                throw new ArgumentException("No local player!");
            }
            return world.ReplaceLocalPlayer(
                player.ChangeExperience(packet.Level, packet.ExperienceBar, packet.TotalExperience)
            );
        }
    }
}
