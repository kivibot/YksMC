using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Client;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Packets;
using YksMC.Protocol.Packets.Login;

namespace YksMC.Bot.Login
{
    public class LoginService : ILoginService
    {
        private readonly IMinecraftClient _minecraftClient;
        private readonly TaskCompletionSource<IPlayerInfo> _taskCompletionSource;

        public LoginService(IMinecraftClient minecraftClient)
        {
            _minecraftClient = minecraftClient;
            _minecraftClient.PacketReceived += OnPacketReceived;
            _taskCompletionSource = new TaskCompletionSource<IPlayerInfo>();
        }

        public async Task<IPlayerInfo> LoginAsync()
        {
            if(_minecraftClient.State != ConnectionState.Handshake)
            {
                throw new InvalidOperationException($"Invalid connection state: {_minecraftClient.State}");
            }

            _minecraftClient.SetState(ConnectionState.Login);
            _minecraftClient.SendPacket(new HandshakePacket()
            {
                NextState = ConnectionState.Login,
                ProtocolVersion = _minecraftClient.ProtocolVersion,
                ServerAddress = _minecraftClient.Address.Host,
                ServerPort = _minecraftClient.Address.Port
            });
            _minecraftClient.SendPacket(new LoginStartPacket()
            {
                Name = "testibotti"
            });

            return await _taskCompletionSource.Task;
        }

        private void OnPacketReceived(object packet)
        {
            if(_minecraftClient.State != ConnectionState.Login)
            {
                return;
            }
            HandlePacket((dynamic)packet);
        }

        private void HandlePacket(object packet)
        {
            throw new ArgumentException("Unsupported packet!");
        }

        private void HandlePacket(LoginSuccessPacket packet)
        {
            PlayerInfo info = new PlayerInfo(packet.UserId, packet.Username);
            _minecraftClient.SetState(ConnectionState.Play);
            _taskCompletionSource.SetResult(info);
        }
    }
}
