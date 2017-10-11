using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Client.EventBus;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Bot.Handlers
{
    public class WorldEventHandlerWrapper : IEventHandler<ChunkDataPacket>
    {
        private readonly ChunkDataHandler _chunkDataHandler;
        private IWorld _world;

        public WorldEventHandlerWrapper(ChunkDataHandler chunkDataHandler, IWorld world)
        {
            _chunkDataHandler = chunkDataHandler;
            _world = world;
        }

        public void Handle(ChunkDataPacket args)
        {
            _world = _chunkDataHandler.ApplyEvent(args, _world);
        }
    }
}
