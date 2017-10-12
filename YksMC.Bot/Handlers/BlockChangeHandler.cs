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
    public class BlockChangeHandler : IWorldEventHandler<BlockChangePacket>
    {
        private readonly IBlockTypeRepository _blockTypeRepository;

        public BlockChangeHandler(IBlockTypeRepository blockTypeRepository)
        {
            _blockTypeRepository = blockTypeRepository;
        }

        public IWorld ApplyEvent(BlockChangePacket packet, IWorld world)
        {
            IBlockTypeIdentity blockTypeId = new BlockTypeIdentity(packet.BlockId >> 4, packet.BlockId & 0b1111);
            IBlockType blockType = _blockTypeRepository.GetBlockType(blockTypeId);
            if (blockType == null)
            {
                throw new ArgumentException($"Invalid block type: {blockTypeId}");
            }

            IDimension dimension = world.GetCurrentDimension();
            if (dimension == null)
            {
                throw new ArgumentException("Dimension is loaded");
            }

            IBlockCoordinate position = new BlockCoordinate(packet.Location.X, packet.Location.Y, packet.Location.Z);
            IChunkCoordinate chunkPosition = new ChunkCoordinate(position);
            IChunk chunk = dimension.GetChunk(chunkPosition);
            IBlock block = chunk.GetBlock(position);

            return world.ReplaceCurrentDimension(dimension.ChangeChunk(chunkPosition, chunk.ChangeBlock(position, block.ChangeType(blockType))));
        }
    }
}
