using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Packets.Login;

namespace YksMC.Client.Handlers
{
    public class LoginHandler : IPacketHandler<DisconnectPacket>, IPacketHandler<EncryptionRequestPacket>, IPacketHandler<SetCompressionPacket>, IPacketHandler<LoginSuccessPacket>
    {
        private readonly IMinecraftClient _client;
        private readonly ILogger _logger;

        public LoginHandler(IMinecraftClient client, ILogger logger)
        {
            _client = client;
            _logger = logger.ForContext<LoginHandler>();
        }

        public async Task HandleAsync(DisconnectPacket packet)
        {
            _logger.Warning("Disconnected: {0}", packet.Reason);
        }

        public Task HandleAsync(EncryptionRequestPacket packet)
        {
            throw new NotImplementedException();
        }

        public Task HandleAsync(SetCompressionPacket packet)
        {
            throw new NotImplementedException();
        }

        public async Task HandleAsync(LoginSuccessPacket packet)
        {
            _logger.Information("Login successful! Username: {username}, UserId: {userId}", packet.Username, packet.UserId);
            _client.SetState(ConnectionState.Play);
        }
    }
}
