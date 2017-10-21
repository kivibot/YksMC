using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.GameObjectRegistry;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Behavior.PacketHandlers
{
    public class BlockChangeHandler : WorldEventHandler, IWorldEventHandler<BlockChangePacket>, IWorldEventHandler<MultiBlockChangePacket>
    {
        private readonly IGameObjectRegistry<IBlock> _blockRegistry;

        public BlockChangeHandler(IGameObjectRegistry<IBlock> blockRegistry)
        {
            _blockRegistry = blockRegistry;
        }

        public IWorldEventResult Handle(IWorldEvent<BlockChangePacket> args)
        {
            IWorld world = args.World;
            BlockChangePacket packet = args.Event;

            IDimension dimension = world.GetCurrentDimension();
            if (dimension == null)
            {
                throw new ArgumentException("Dimension is loaded");
            }

            IBlockLocation position = new BlockLocation(packet.Location.X, packet.Location.Y, packet.Location.Z);
            IChunkCoordinate chunkPosition = new ChunkCoordinate(position);
            IChunk chunk = dimension.GetChunk(chunkPosition);
            chunk = ReplaceBlockType(chunk, position, packet.BlockId);

            return Result(world.ReplaceCurrentDimension(dimension.ReplaceChunk(chunkPosition, chunk)));
        }

        public IWorldEventResult Handle(IWorldEvent<MultiBlockChangePacket> args)
        {
            IWorld world = args.World;
            MultiBlockChangePacket packet = args.Event;

            IDimension dimension = world.GetCurrentDimension();
            if (dimension == null)
            {
                throw new ArgumentException("Dimension is loaded");
            }

            IChunkCoordinate chunkPosition = new ChunkCoordinate(packet.ChunkX, packet.ChunkZ);
            IChunk chunk = dimension.GetChunk(chunkPosition);

            foreach (MultiBlockChangePacketRecord record in packet.Records.Values)
            {
                IBlockLocation position = new BlockLocation(record.HorizontalPosition >> 4, record.Y, record.HorizontalPosition & 0xF);
                chunk = ReplaceBlockType(chunk, position, record.BlockId);
            }

            return Result(world.ReplaceCurrentDimension(dimension.ReplaceChunk(chunkPosition, chunk)));
        }

        private IChunk ReplaceBlockType(IChunk chunk, IBlockLocation position, int networkId)
        {
            int blockId = networkId >> 4;
            byte blockMetadata = (byte)(networkId & 0b1111);

            IBlock oldBlock = chunk.GetBlock(position);

            IBlock block = _blockRegistry.Get<IBlock>(blockId)
                .WithDataValue(blockMetadata)
                .WithBiome(oldBlock.Biome)
                .WithLightFromBlocks(oldBlock.LightFromBlocks)
                .WithLightFromSky(oldBlock.LightFromSky);

            return chunk.ChangeBlock(position, block);
        }
    }
}
