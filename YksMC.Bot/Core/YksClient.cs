using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.Login;
using YksMC.Bot.PacketHandlers;
using YksMC.Bot.TickHandlers;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Packets.Play.Serverbound;

namespace YksMC.Bot.Core
{
    [Obsolete("Old loop")]
    public class YksClient
    {
        private readonly IMinecraftClient _minecraftClient;
        private readonly ILoginService _loginService;
        private readonly WorldEventHandlerWrapper _worldEventHandlerWrapper;
        private readonly PlayerMovementHandler _playerMovementHandler;

        private readonly ConcurrentQueue<object> _packetQueue;

        private IWorld _world;

        public YksClient(IMinecraftClient minecraftClient, ILoginService loginService, IWorld world, WorldEventHandlerWrapper worldEventHandlerWrapper, PlayerMovementHandler playerMovementHandler)
        {
            _minecraftClient = minecraftClient;
            _minecraftClient.PacketReceived += OnPacketReceived;
            _loginService = loginService;
            _packetQueue = new ConcurrentQueue<object>();
            _worldEventHandlerWrapper = worldEventHandlerWrapper;
            _playerMovementHandler = playerMovementHandler;
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
            _world = _world.ReplaceLocalPlayer(new Player(Guid.Parse(playerInfo.Id), playerInfo.Username));
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

            _world = _playerMovementHandler.ApplyEvent(null, _world).World;

            IPlayer player = _world.GetLocalPlayer();
            if (!player.HasEntity)
            {
                return;
            }
            IEntity entity = _world.GetCurrentDimension().GetEntity(player.EntityId);
            bool onGround = (entity.Location.Y - Math.Round(entity.Location.Y)) == 0;
            _minecraftClient.SendPacket(new PlayerPositionPacket() { X = entity.Location.X, FeetY = entity.Location.Y, Z = entity.Location.Z, OnGround = entity.IsOnGround });
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
