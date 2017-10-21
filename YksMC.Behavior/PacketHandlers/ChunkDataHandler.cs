using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.GameObjectRegistry;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Biome;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;
using YksMC.Protocol;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Behavior.PacketHandlers
{
    public class ChunkDataHandler : WorldEventHandler, IWorldEventHandler<ChunkDataPacket>
    {
        private const int _primaryBitMaskBits = 32;
        private const int _sectionWidth = 16;
        private const int _sectionHeight = 16;

        private readonly IPacketReader _reader;
        private readonly IGameObjectRegistry<IBlock> _blockRegistry;
        private readonly IBiomeRepository _biomeRepository;

        public ChunkDataHandler(IPacketReader packetReader, IGameObjectRegistry<IBlock> blocks, IBiomeRepository biomeRepository)
        {
            _reader = packetReader;
            _blockRegistry = blocks;
            _biomeRepository = biomeRepository;
        }

        public IWorldEventResult Handle(IWorldEvent<ChunkDataPacket> message)
        {
            IWorld world = message.World;
            ChunkDataPacket packet = message.Event;

            _reader.SetPacket(packet.DataAndBiomes.Values);

            IDimension dimension = world.GetCurrentDimension();

            IChunkCoordinate position = new ChunkCoordinate(packet.ChunkX, packet.ChunkZ);
            IChunk chunk = dimension.GetChunk(position);

            chunk = ParseChunk(packet, chunk, dimension.Type);

            return Result(world.ReplaceCurrentDimension(dimension.ReplaceChunk(position, chunk)));
        }

        private IChunk ParseChunk(ChunkDataPacket packet, IChunk chunk, IDimensionType dimensionType)
        {
            for (int sectionY = 0; sectionY < _primaryBitMaskBits; sectionY++)
            {
                int sectionBit = (packet.PrimaryBitMask >> sectionY) & 0b1;
                if (sectionBit == 0)
                {
                    continue;
                }
                chunk = ParseChunkSection(packet, chunk, sectionY, dimensionType);
            }
            return chunk;
        }

        private IChunk ParseChunkSection(ChunkDataPacket packet, IChunk chunk, int sectionY, IDimensionType dimensionType)
        {
            byte bitsPerBlock = GetBitsPerBlock();
            int[] typePalette = GetBlockTypePalette();
            ulong[] typeData = GetBlockTypeData();
            byte[] lightData = GetLightData(dimensionType);

            for (int localY = 0; localY < _sectionHeight; localY++)
            {
                for (int z = 0; z < _sectionWidth; z++)
                {
                    for (int x = 0; x < _sectionWidth; x++)
                    {
                        IBlockLocation position = new BlockLocation(x, sectionY * _sectionHeight + localY, z);

                        IBlock block = ParseType(position, bitsPerBlock, typePalette, typeData);
                        block = ParseLightLevels(block, position, lightData, dimensionType);
                        if (packet.GroundUpContinuous)
                        {
                            block = ParseBiome(packet, block, position);
                        }

                        chunk = chunk.ChangeBlock(position, block);
                    }
                }
            }

            return chunk;
        }

        private byte GetBitsPerBlock()
        {
            byte bitsPerBlock = _reader.GetByte();
            return bitsPerBlock;
        }

        private int[] GetBlockTypePalette()
        {
            int paletteLenght = _reader.GetVarInt();
            int[] palette = new int[paletteLenght];
            for (int i = 0; i < paletteLenght; i++)
            {
                palette[i] = _reader.GetVarInt();
            }
            return palette;
        }

        private ulong[] GetBlockTypeData()
        {
            int dataLength = _reader.GetVarInt();
            ulong[] data = new ulong[dataLength];
            for (int i = 0; i < dataLength; i++)
            {
                data[i] = _reader.GetUnsignedLong();
            }
            return data;
        }

        private byte[] GetLightData(IDimensionType dimensionType)
        {
            int bytes = _sectionHeight * _sectionWidth * _sectionWidth;
            if (!dimensionType.HasSkylight)
            {
                bytes /= 2;
            }
            return _reader.GetBytes(bytes);
        }

        private IBlock ParseType(IBlockLocation position, int bitsPerBlock, int[] typePalette, ulong[] typeData)
        {
            int blockIndex = ((((position.Y % _sectionHeight) * _sectionHeight) + position.Z) * _sectionWidth + position.X);
            int bitsPerUlong = sizeof(ulong) * 8;
            int bitOffset = (blockIndex * bitsPerBlock) % bitsPerUlong;
            int offset = (blockIndex * bitsPerBlock) / bitsPerUlong;

            ulong paletteIndex = typeData[offset] >> bitOffset;

            int missingBits = (bitOffset + bitsPerBlock) - bitsPerUlong;
            if (missingBits > 0)
            {
                paletteIndex |= typeData[offset + 1] << (bitsPerBlock - missingBits);
            }

            paletteIndex &= (ulong)((1 << bitsPerBlock) - 1);

            int paletteValue;
            if (typePalette.Length > 0)
            {
                paletteValue = typePalette[paletteIndex];
            }
            else
            {
                paletteValue = (int)paletteIndex;
            }

            int typeId = paletteValue >> 4;
            byte metadata = (byte)(paletteValue & 0b1111);

            IBlock block = _blockRegistry.Get<IBlock>(typeId)
                .WithDataValue(metadata);
            return block;
        }

        private IBlock ParseLightLevels(IBlock block, IBlockLocation position, byte[] lightData, IDimensionType dimensionType)
        {
            int lightIndex = (((position.Y % _sectionHeight) * _sectionHeight) + position.Z) * _sectionWidth + position.X;
            int lightBitOffset = 4 * (lightIndex % 2);
            int lightLevel = (lightData[lightIndex / 2] >> lightBitOffset) & 0b1111;

            block = block.WithLightFromBlocks((byte)lightLevel);

            if (!dimensionType.HasSkylight)
            {
                return block;
            }

            int skylightOffset = (_sectionHeight * _sectionWidth * _sectionWidth) / 2;
            int skylightLevel = (lightData[skylightOffset + lightIndex / 2] >> lightBitOffset) & 0b1111;

            return block.WithLightFromSky((byte)skylightLevel);
        }

        private IBlock ParseBiome(ChunkDataPacket packet, IBlock block, IBlockLocation position)
        {
            if (!packet.GroundUpContinuous)
            {
                return block;
            }
            int biomeDataOffset = packet.DataAndBiomes.Count - (_sectionWidth * _sectionWidth);
            int biomeIndex = position.Z * _sectionWidth + position.X;
            byte biomeId = packet.DataAndBiomes[biomeDataOffset + biomeIndex];
            IBiome biome = _biomeRepository.GetBiome(biomeId);
            return block.WithBiome(biome);
        }
    }
}
