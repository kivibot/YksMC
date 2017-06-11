using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Abstraction.Bot;
using YksMC.Abstraction.Misc;
using YksMC.Abstraction.World;
using YksMC.Client;
using YksMC.Client.EventBus;
using YksMC.Protocol.Packets.Play.Clientbound;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Bot.Handlers
{
    public class PlayerHandler : IEventHandler<JoinGamePacket>, IEventHandler<PlayerPositionLookPacket>
    {
        private readonly IEntityService _entityService;
        private readonly IMinecraftClient _client;
        private readonly IMinecraftBot _bot;

        public PlayerHandler(IEntityService entityService, IMinecraftClient client, IMinecraftBot bot)
        {
            _entityService = entityService;
            _client = client;
            _bot = bot;
        }

        public void Handle(JoinGamePacket packet)
        {
            //TODO: the dimension should not be casted from an int
            IPlayer player = _entityService.CreatePlayer(packet.EntityId, _bot.UserId, (Dimension)packet.Dimension);
            _bot.SetPlayer(player);
        }

        public void Handle(PlayerPositionLookPacket packet)
        {
            UpdatePlayerPosition(packet);
            UpdatePlayerLookDirection(packet);

            _client.SendPacket(new TeleportConfirmPacket()
            {
                TeleportId = packet.TeleportId
            });
        }

        private void UpdatePlayerLookDirection(PlayerPositionLookPacket packet)
        {
            IPlayer player = _bot.Player;
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
            _entityService.SetPlayerLookDirection(player.EntityId, new LookDirection(pitch, yaw));
        }

        private void UpdatePlayerPosition(PlayerPositionLookPacket packet)
        {
            IPlayer player = _bot.Player;
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
            _entityService.SetPlayerPosition(player.EntityId, new Vector3<double>(x, y, z));
        }
    }
}
