using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Abstraction.Models;
using YksMC.Abstraction.Services;
using YksMC.Client;
using YksMC.Client.Handler;
using YksMC.Protocol.Packets.Play.Clientbound;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Bot.Handlers
{
    public class PlayerHandler : IEventHandler<JoinGamePacket>, IEventHandler<PlayerPositionLookPacket>
    {
        private readonly IEntityService _entityService;
        private readonly IMinecraftClient _client;

        public PlayerHandler(IEntityService entityService, IMinecraftClient client)
        {
            _entityService = entityService;
            _client = client;
        }

        public async Task HandleAsync(JoinGamePacket packet)
        {
            _entityService.CreatePlayer(packet.EntityId, (Dimension)packet.Dimension, packet.Gamemode);
        }

        public async Task HandleAsync(PlayerPositionLookPacket packet)
        {
            IPlayer player = _entityService.GetPlayer();

            double x = packet.X;
            double y = packet.FeetY;
            double z = packet.Z;
            if (packet.Flags.RelativeX)
            {
                x += player.Position.X;
            }
            if (packet.Flags.RelativeY)
            {
                y += player.Position.Y;
            }
            if (packet.Flags.RelativeZ)
            {
                z += player.Position.Z;
            }
            _entityService.UpdatePlayerPosition(new Vector3<double>(x, y, z));

            double yaw = packet.Yaw;
            double pitch = packet.Pitch;
            if (packet.Flags.RelativeYaw)
            {
                yaw += player.LookDirection.Yaw;
            }
            if (packet.Flags.RelativePitch)
            {
                pitch += player.LookDirection.Pitch;

            }
            _entityService.UpdatePlayerLookDirection(new LookDirection(pitch, yaw));

            _client.SendPacket(new TeleportConfirmPacket()
            {
                TeleportId = packet.TeleportId
            });
        }
    }
}
