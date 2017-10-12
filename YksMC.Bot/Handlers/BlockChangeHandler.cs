using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.BlockType;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Bot.Handlers
{
    public class BlockChangeHandler : IWorldEventHandler<BlockChangePacket>, IWorldEventHandler<MultiBlockChangePacket>
    {
        private readonly IBlockTypeRepository _blockTypeRepository;

        public BlockChangeHandler(IBlockTypeRepository blockTypeRepository)
        {
            _blockTypeRepository = blockTypeRepository;
        }

        public IWorld ApplyEvent(BlockChangePacket packet, IWorld world)
        {
            IDimension dimension = world.GetCurrentDimension();
            if (dimension == null)
            {
                throw new ArgumentException("Dimension is loaded");
            }

            IBlockCoordinate position = new BlockCoordinate(packet.Location.X, packet.Location.Y, packet.Location.Z);
            IChunkCoordinate chunkPosition = new ChunkCoordinate(position);
            IChunk chunk = dimension.GetChunk(chunkPosition);
            chunk = ReplaceBlockType(chunk, position, packet.BlockId);

            return world.ReplaceCurrentDimension(dimension.ChangeChunk(chunkPosition, chunk));
        }

        public IWorld ApplyEvent(MultiBlockChangePacket packet, IWorld world)
        {
            IDimension dimension = world.GetCurrentDimension();
            if (dimension == null)
            {
                throw new ArgumentException("Dimension is loaded");
            }

            IChunkCoordinate chunkPosition = new ChunkCoordinate(packet.ChunkX, packet.ChunkZ);
            IChunk chunk = dimension.GetChunk(chunkPosition);

            foreach (MultiBlockChangePacketRecord record in packet.Records.Values)
            {
                IBlockCoordinate position = new BlockCoordinate(record.HorizontalPosition >> 4, record.Y, record.HorizontalPosition & 0xF);
                chunk = ReplaceBlockType(chunk, position, record.BlockId);
            }

            return world.ReplaceCurrentDimension(dimension.ChangeChunk(chunkPosition, chunk));
        }

        private IChunk ReplaceBlockType(IChunk chunk, IBlockCoordinate position, int networkId)
        {
            IBlockTypeIdentity blockTypeId = new BlockTypeIdentity(networkId >> 4, networkId & 0b1111);
            IBlockType blockType = _blockTypeRepository.GetBlockType(blockTypeId);
            if (blockType == null)
            {
                throw new ArgumentException($"Invalid block type: {blockTypeId}");
            }
            IBlock block = chunk.GetBlock(position)
                .ChangeType(blockType);
            return chunk.ChangeBlock(position, block);
        }
    }
}
