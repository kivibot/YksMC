using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Client.EventBus;
using YksMC.MinecraftModel.Dimension;
using YksMC.Protocol.Packets.Login;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Bot.Handlers
{
    public class WorldEventHandlerWrapper : IEventHandler<ChunkDataPacket>, IEventHandler<JoinGamePacket>, IEventHandler<PlayerPositionLookPacket>, IEventHandler<LoginSuccessPacket>
    {
        private readonly ChunkDataHandler _chunkDataHandler;
        private readonly PlayerHandler _playerHandler;
        private readonly LoginHandler _loginHandler;
        private IDimension _dimension;

        public WorldEventHandlerWrapper(ChunkDataHandler chunkDataHandler, PlayerHandler playerHandler, LoginHandler loginHandler, IDimension dimension)
        {
            _chunkDataHandler = chunkDataHandler;
            _playerHandler = playerHandler;
            _loginHandler = loginHandler;
            _dimension = dimension;
        }

        public void Handle(ChunkDataPacket args)
        {
            _dimension = _chunkDataHandler.ApplyEvent(args, _dimension);
        }

        public void Handle(JoinGamePacket args)
        {
            _dimension = _playerHandler.ApplyEvent(args, _dimension);
        }

        public void Handle(PlayerPositionLookPacket args)
        {
            _dimension = _playerHandler.ApplyEvent(args, _dimension);
        }

        public void Handle(LoginSuccessPacket args)
        {
            _dimension = _loginHandler.ApplyEvent(args, _dimension);
        }
    }
}
