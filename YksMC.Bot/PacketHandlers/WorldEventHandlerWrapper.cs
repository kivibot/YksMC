using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.WorldEvent;
using YksMC.Client;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Login;
using YksMC.Protocol.Packets.Play.Clientbound;
using YksMC.Protocol.Packets.Play.Common;

namespace YksMC.Bot.PacketHandlers
{
    [Obsolete("Will hopefully be replaced")]
    public class WorldEventHandlerWrapper : WorldEventHandler, IWorldEventHandler<object>
    {
        private readonly ChunkDataHandler _chunkDataHandler;
        private readonly PlayerHandler _playerHandler;
        private readonly TimeUpdateHandler _timeUpdateHandler;
        private readonly BlockChangeHandler _blockChangeHandler;
        private readonly EntityHandler _entityHandler;
        private readonly KeepAliveHandler _keepAliveHandler;

        public WorldEventHandlerWrapper(ChunkDataHandler chunkDataHandler, PlayerHandler playerHandler, TimeUpdateHandler timeUpdateHandler,
            BlockChangeHandler blockChangeHandler, EntityHandler entityHandler, KeepAliveHandler keepAliveHandler)
        {
            _chunkDataHandler = chunkDataHandler;
            _playerHandler = playerHandler;
            _timeUpdateHandler = timeUpdateHandler;
            _blockChangeHandler = blockChangeHandler;
            _entityHandler = entityHandler;
            _keepAliveHandler = keepAliveHandler;
        }

        public IWorldEventResult ApplyEvent(object packet, IWorld world)
        {
            return Handle((dynamic)packet, world);
        }

        public IWorldEventResult Handle(object args, IWorld world)
        {
            return Result(world);
        }

        private IWorldEventResult Handle(ChunkDataPacket args, IWorld world)
        {
            return _chunkDataHandler.ApplyEvent(args, world);
        }

        private IWorldEventResult Handle(JoinGamePacket args, IWorld world)
        {
            return _playerHandler.ApplyEvent(args, world);
        }

        private IWorldEventResult Handle(PlayerPositionLookPacket args, IWorld world)
        {
            return _playerHandler.ApplyEvent(args, world);
        }

        private IWorldEventResult Handle(TimeUpdatePacket args, IWorld world)
        {
            return _timeUpdateHandler.ApplyEvent(args, world);
        }

        private IWorldEventResult Handle(BlockChangePacket args, IWorld world)
        {
            return _blockChangeHandler.ApplyEvent(args, world);
        }

        private IWorldEventResult Handle(SetExperiencePacket args, IWorld world)
        {
            return _playerHandler.ApplyEvent(args, world);
        }

        private IWorldEventResult Handle(SpawnMobPacket args, IWorld world)
        {
            return _entityHandler.ApplyEvent(args, world);
        }

        private IWorldEventResult Handle(KeepAlivePacket args, IWorld world)
        {
            return _keepAliveHandler.ApplyEvent(args, world);
        }

        private IWorldEventResult Handle(MultiBlockChangePacket args, IWorld world)
        {
            return _blockChangeHandler.ApplyEvent(args, world);
        }

        private IWorldEventResult Handle(SpawnPlayerPacket args, IWorld world)
        {
            return _playerHandler.ApplyEvent(args, world);
        }

        private IWorldEventResult Handle(EntityRelativeMovePacket args, IWorld world)
        {
            return _entityHandler.ApplyEvent(args, world);
        }

        private IWorldEventResult Handle(EntityLookAndRelativeMovePacket args, IWorld world)
        {
            return _entityHandler.ApplyEvent(args, world);
        }
    }
}
