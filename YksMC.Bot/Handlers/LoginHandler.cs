using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Client;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.Dimension;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Packets.Login;
using YksMC.Protocol.Packets.Play.Clientbound;
using YksMC.Protocol.Packets.Play.Serverbound;
using YksMC.MinecraftModel.World;
using YksMC.Bot.WorldEvent;

namespace YksMC.Bot.Handlers
{
    [Obsolete("TODO")]
    public class LoginHandler : WorldEventHandler, IEventHandler<DisconnectPacket>, IEventHandler<EncryptionRequestPacket>, IEventHandler<SetCompressionPacket>, IWorldEventHandler<LoginSuccessPacket>
    {
        private readonly IMinecraftClient _client;
        private readonly ILogger _logger;

        public LoginHandler(IMinecraftClient client, ILogger logger)
        {
            _client = client;
            _logger = logger.ForContext<LoginHandler>();
        }

        public async void Handle(DisconnectPacket packet)
        {
            _logger.Warning("Disconnected: {0}", packet.Reason);
            _client.Disconnect();
        }

        public void Handle(EncryptionRequestPacket packet)
        {
            throw new NotImplementedException();
        }

        public void Handle(SetCompressionPacket packet)
        {
            throw new NotImplementedException();
        }

        public IWorldEventResult ApplyEvent(LoginSuccessPacket packet, IWorld world)
        {
            _logger.Information("Login successful! Username: {username}, UserId: {userId}", packet.Username, packet.UserId);
            _client.SetState(ConnectionState.Play);
            
            ClientStatusPacket statusReply = new ClientStatusPacket()
            {
                ActionId = 0
            };
            ClientSettingsPacket settingsReply = new ClientSettingsPacket()
            {
                Locale = "en_GB",
                ViewDistance = 32,
                ChatMode = 0,
                UseChatColors = false,
                DisplayedSkinParts = 0b01111111,
                MainHand = 1
            };

            IPlayer player = new Player(new PlayerId(packet.UserId), packet.Username);
            return Result(world.ReplaceLocalPlayer(player), statusReply, settingsReply);
        }
    }
}
