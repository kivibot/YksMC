using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.Handlers;
using YksMC.Bot.Login;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Bot.Core
{
    public class YksClient
    {
        private readonly IMinecraftClient _minecraftClient;
        private readonly ILoginService _loginService;
        private readonly WorldEventHandlerWrapper _worldEventHandlerWrapper;

        private readonly ConcurrentQueue<object> _packetQueue;

        private IWorld _world;

        public YksClient(IMinecraftClient minecraftClient, ILoginService loginService, IWorld world, WorldEventHandlerWrapper worldEventHandlerWrapper)
        {
            _minecraftClient = minecraftClient;
            _minecraftClient.PacketReceived += OnPacketReceived;
            _loginService = loginService;
            _packetQueue = new ConcurrentQueue<object>();
            _worldEventHandlerWrapper = worldEventHandlerWrapper;
            _world = world;
        }

        public async Task RunAsync()
        {
            await ConnectAndLoginAsync();
            await LoopAsync();
        }

        private async Task ConnectAndLoginAsync()
        {
            await _minecraftClient.ConnectAsync("localhost", 25565);
            IPlayerInfo playerInfo = await _loginService.LoginAsync();
            //TODO: should this be removed from here?
            _world = _world.ReplaceLocalPlayer(new Player(new PlayerId(playerInfo.Id), playerInfo.Username));
        }

        public async Task LoopAsync()
        {
            while (_minecraftClient.State == ConnectionState.Play)
            {
                LoopSingle();
                await Task.Delay(50);
            }
        }

        public void LoopSingle()
        {
            while (_packetQueue.TryDequeue(out object packet))
            {
                IWorldEventResult result = _worldEventHandlerWrapper.ApplyEvent(packet, _world);
                foreach (object replyPacket in result.ReplyPackets)
                {
                    _minecraftClient.SendPacket(replyPacket);
                }
                _world = result.World;
            }
            _minecraftClient.SendPacket(new PlayerPacket() { OnGround = true });
        }

        private void OnPacketReceived(object packet)
        {
            if (_minecraftClient.State != ConnectionState.Play)
            {
                return;
            }
            _packetQueue.Enqueue(packet);
        }
    }
}
