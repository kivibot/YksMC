using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.Client.EventBus;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Login;
using YksMC.Protocol.Packets.Play.Clientbound;
using YksMC.Protocol.Packets.Play.Common;

namespace YksMC.Bot.Handlers
{
    public class WorldEventHandlerWrapper : IEventHandler<ChunkDataPacket>, IEventHandler<JoinGamePacket>,
        IEventHandler<PlayerPositionLookPacket>, IEventHandler<LoginSuccessPacket>, IEventHandler<TimeUpdatePacket>,
        IEventHandler<BlockChangePacket>, IEventHandler<SetExperiencePacket>, IEventHandler<SpawnMobPacket>,
        IEventHandler<KeepAlivePacket>
    {
        private readonly ChunkDataHandler _chunkDataHandler;
        private readonly PlayerHandler _playerHandler;
        private readonly LoginHandler _loginHandler;
        private readonly TimeUpdateHandler _timeUpdateHandler;
        private readonly BlockChangeHandler _blockChangeHandler;
        private readonly EntityHandler _entityHandler;
        private readonly KeepAliveHandler _keepAliveHandler;

        private readonly IMinecraftClient _client;
        private IWorld _world;

        public WorldEventHandlerWrapper(ChunkDataHandler chunkDataHandler, PlayerHandler playerHandler, 
            LoginHandler loginHandler, TimeUpdateHandler timeUpdateHandler, BlockChangeHandler blockChangeHandler, 
            EntityHandler entityHandler, KeepAliveHandler keepAliveHandler,
            IMinecraftClient client, IWorld dimension)
        {
            _chunkDataHandler = chunkDataHandler;
            _playerHandler = playerHandler;
            _loginHandler = loginHandler;
            _timeUpdateHandler = timeUpdateHandler;
            _blockChangeHandler = blockChangeHandler;
            _entityHandler = entityHandler;
            _keepAliveHandler = keepAliveHandler;

            _client = client;
            _world = dimension;
        }

        public void Handle<T>(T packet, IWorldEventHandler<T> handler)
        {
            IWorldEventResult result = handler.ApplyEvent(packet, _world);
            foreach(object replyPacket in result.ReplyPackets)
            {
                _client.SendPacket(replyPacket);
            }
            _world = result.World;
        }

        public void Handle(ChunkDataPacket args)
        {
            Handle(args, _chunkDataHandler);
        }

        public void Handle(JoinGamePacket args)
        {
            Handle(args, _playerHandler);
        }

        public void Handle(PlayerPositionLookPacket args)
        {
            Handle(args, _playerHandler);
        }

        public void Handle(LoginSuccessPacket args)
        {
            Handle(args, _loginHandler);
        }

        public void Handle(TimeUpdatePacket args)
        {
            Handle(args, _timeUpdateHandler);
        }

        public void Handle(BlockChangePacket args)
        {
            Handle(args, _blockChangeHandler);
        }

        public void Handle(SetExperiencePacket args)
        {
            Handle(args, _playerHandler);
        }

        public void Handle(SpawnMobPacket args)
        {
            Handle(args, _entityHandler);
        }

        public void Handle(KeepAlivePacket args)
        {
            Handle(args, _keepAliveHandler);
        }
    }
}
