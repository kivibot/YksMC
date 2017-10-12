using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Client.EventBus;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Login;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Bot.Handlers
{
    public class WorldEventHandlerWrapper : IEventHandler<ChunkDataPacket>, IEventHandler<JoinGamePacket>, IEventHandler<PlayerPositionLookPacket>, IEventHandler<LoginSuccessPacket>, IEventHandler<TimeUpdatePacket>
    {
        private readonly ChunkDataHandler _chunkDataHandler;
        private readonly PlayerHandler _playerHandler;
        private readonly LoginHandler _loginHandler;
        private readonly TimeUpdateHandler _timeUpdateHandler;
        private IWorld _world;

        public WorldEventHandlerWrapper(ChunkDataHandler chunkDataHandler, PlayerHandler playerHandler, LoginHandler loginHandler, TimeUpdateHandler timeUpdateHandler, IWorld dimension)
        {
            _chunkDataHandler = chunkDataHandler;
            _playerHandler = playerHandler;
            _loginHandler = loginHandler;
            _timeUpdateHandler = timeUpdateHandler;
            _world = dimension;
        }

        public void Handle(ChunkDataPacket args)
        {
            _world = _chunkDataHandler.ApplyEvent(args, _world);
        }

        public void Handle(JoinGamePacket args)
        {
            _world = _playerHandler.ApplyEvent(args, _world);
        }

        public void Handle(PlayerPositionLookPacket args)
        {
            _world = _playerHandler.ApplyEvent(args, _world);
        }

        public void Handle(LoginSuccessPacket args)
        {
            _world = _loginHandler.ApplyEvent(args, _world);
        }

        public void Handle(TimeUpdatePacket args)
        {
            _world = _timeUpdateHandler.ApplyEvent(args, _world);
        }
    }
}
