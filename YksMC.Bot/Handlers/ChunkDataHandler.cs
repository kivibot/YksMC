using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Biome;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.World;
using YksMC.Protocol;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Bot.Handlers
{
    public class ChunkDataHandler : IWorldEventHandler<ChunkDataPacket>
    {
        private const int _primaryBitMaskBits = 32;
        private const int _sectionWidth = 16;
        private const int _sectionHeight = 16;

        private readonly IPacketReader _reader;
        private readonly IBiomeRepository _biomeRepository;

        public IWorld ApplyEvent(ChunkDataPacket packet, IWorld world)
        {
            _reader.SetPacket(packet.DataAndBiomes.Values);

            IChunkCoordinate position = new ChunkCoordinate(packet.ChunkX, packet.ChunkZ);
            IChunk chunk = world.GetChunk(position);

            chunk = ParseChunk(packet, chunk);

            return world.ChangeChunk(position, chunk);
        }

        private IChunk ParseChunk(ChunkDataPacket packet, IChunk chunk)
        {
            for (int sectionY = 0; sectionY < _primaryBitMaskBits; sectionY++)
            {
                int sectionBit = (packet.PrimaryBitMask >> sectionY) & 0b1;
                if (sectionBit == 0)
                {
                    continue;
                }
                chunk = ParseChunkSection(packet, chunk, sectionY);
            }
            return chunk;
        }

        private IChunk ParseChunkSection(ChunkDataPacket packet, IChunk chunk, int sectionY)
        {
            byte bitsPerBlock = GetBitsPerBlock();
            int[] typePalette = GetBlockTypePalette();
            ulong[] typeData = GetBlockTypeData();
            byte[] lightData = GetLightData(chunk);

            for (int localY = 0; localY < _sectionHeight; localY++)
            {
                for (int z = 0; z < _sectionWidth; z++)
                {
                    for (int x = 0; x < _sectionWidth; x++)
                    {
                        IBlockCoordinate position = new BlockCoordinate(x, sectionY * _sectionHeight + localY, z);
                        IBlock block = chunk.GetBlock(position);

                        block = ParseType(block, position, bitsPerBlock, typePalette, typeData);
                        block = ParseLightLevels(block, position, lightData);
                        if (packet.GroundUpContinuous)
                        {
                            block = ParseBiome(packet, block, position);
                        }

                        chunk = chunk.ChangeBlock(position, block);
                    }
                }
            }
        }

        private IBlock ParseBiome(ChunkDataPacket packet, IBlock block, IBlockCoordinate position)
        {
            if (!packet.GroundUpContinuous)
            {
                return block;
            }
            int biomeDataOffset = packet.DataAndBiomes.Count - (_sectionWidth * _sectionWidth);
            int biomeIndex = position.Z * _sectionWidth + position.X;
            byte biomeId = packet.DataAndBiomes[biomeDataOffset + biomeIndex];
            IBiome biome = _biomeRepository.GetBiome(biomeId);
            return block.ChangeBiome(biome);
        }
    }
}
