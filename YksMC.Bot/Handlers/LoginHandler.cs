using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Abstraction.Bot;
using YksMC.Client;
using YksMC.Client.EventBus;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Packets.Login;
using YksMC.Protocol.Packets.Play.Clientbound;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Bot.Handlers
{
    public class LoginHandler : IEventHandler<DisconnectPacket>, IEventHandler<EncryptionRequestPacket>, IEventHandler<SetCompressionPacket>, IEventHandler<LoginSuccessPacket>
    {
        private readonly IMinecraftClient _client;
        private readonly ILogger _logger;
        private readonly IMinecraftBot _bot;

        public LoginHandler(IMinecraftClient client, ILogger logger, IMinecraftBot bot)
        {
            _client = client;
            _logger = logger.ForContext<LoginHandler>();
            _bot = bot;
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

        public void Handle(LoginSuccessPacket packet)
        {
            _logger.Information("Login successful! Username: {username}, UserId: {userId}", packet.Username, packet.UserId);
            _client.SetState(ConnectionState.Play);
            _bot.SetUserInfo(Guid.Parse(packet.UserId), packet.Username);

            //TODO: remove
            _client.SendPacket(new ClientStatusPacket()
            {
                ActionId = 0
            });
            _client.SendPacket(new ClientSettingsPacket()
            {
                Locale = "en_GB",
                ViewDistance = 32,
                ChatMode = 0,
                UseChatColors = false,
                DisplayedSkinParts = 0b01111111,
                MainHand = 1
            });
        }
    }
}
