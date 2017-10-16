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
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Common;
using System.Linq;

namespace YksMC.Bot.PacketHandlers
{
    public class PlayerHandler : WorldEventHandler, IWorldEventHandler<JoinGamePacket>, IWorldEventHandler<PlayerPositionLookPacket>,
        IWorldEventHandler<SetExperiencePacket>
    {
        private readonly IEntityTypeRepository _entityTypeRepository;

        public PlayerHandler(IEntityTypeRepository entityTypeRepository)
        {
            _entityTypeRepository = entityTypeRepository;
        }

        public IWorldEventResult ApplyEvent(JoinGamePacket packet, IWorld world)
        {
            IEntityType playerEntityType = _entityTypeRepository.GetPlayerType();
            IEntity playerEntity = new Entity(packet.EntityId, playerEntityType, EntityLocation.Origin, 0, 0, 0, false, new Vector3d(0, 0, 0));

            IDimension dimension = world.GetDimension(packet.Dimension)
                .ChangeEntity(playerEntity);

            IPlayer player = world.GetLocalPlayer()
                .ChangeEntity(playerEntity.Id, dimension.Id);

            world = world.ReplaceCurrentDimension(dimension)
                .ReplacePlayer(player);
            return Result(world);
        }

        public IWorldEventResult ApplyEvent(PlayerPositionLookPacket packet, IWorld world)
        {
            IDimension dimension = world.GetCurrentDimension();
            if (dimension == null)
            {
                throw new ArgumentException("No current dimension!");
            }
            IPlayer player = world.GetLocalPlayer();
            if (player == null)
            {
                throw new ArgumentException("Local player not initialized.");
            }
            if (!player.HasEntity)
            {
                throw new ArgumentException("Local player not spawned.");
            }
            IEntity playerEntity = dimension.GetEntity(player.EntityId);

            PlayerPositionLookPacketFlags flags = packet.Flags;
            double x = (flags.RelativeX ? playerEntity.Location.X : 0) + packet.X;
            double y = (flags.RelativeY ? playerEntity.Location.Y : 0) + packet.FeetY;
            double z = (flags.RelativeZ ? playerEntity.Location.Z : 0) + packet.Z;
            double yaw = (flags.RelativeYaw ? playerEntity.Yaw : 0) + packet.Yaw;
            double pitch = (flags.RelativePitch ? playerEntity.Pitch : 0) + packet.Pitch;

            playerEntity = playerEntity.ChangeLocation(new EntityLocation(x, y, z))
                .ChangeLook(yaw, pitch);

            TeleportConfirmPacket confirmationPacket = new TeleportConfirmPacket()
            {
                TeleportId = packet.TeleportId
            };

            world = world.ReplaceCurrentDimension(dimension.ChangeEntity(playerEntity));
            return Result(world, confirmationPacket);
        }

        public IWorldEventResult ApplyEvent(SetExperiencePacket packet, IWorld world)
        {
            IPlayer player = world.GetLocalPlayer();
            if (player == null)
            {
                throw new ArgumentException("No local player!");
            }
            world = world.ReplaceLocalPlayer(
                player.ChangeExperience(packet.Level, packet.ExperienceBar, packet.TotalExperience)
            );
            return Result(world);
        }

        public IWorldEventResult ApplyEvent(SpawnPlayerPacket packet, IWorld world)
        {
            if(world.GetPlayers().Any(p => p.Id == packet.PlayerId))
            {
                return Result(world);
            }
            
            IEntityType playerEntityType = _entityTypeRepository.GetPlayerType();
            IEntity playerEntity = new Entity(packet.EntityId, playerEntityType, new EntityLocation(packet.Position.X, packet.Position.Y, packet.Position.Z), 0, 0, 0, false, new Vector3d(0, 0, 0));

            IPlayer player = new Player(packet.PlayerId, "<Unknown>")
                .ChangeEntity(playerEntity.Id, world.GetCurrentDimension().Id);

            return Result(world.ReplacePlayer(player).ChangeCurrentDimension(dimension => dimension.ChangeEntity(playerEntity)));
        }
    }
}
