﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Biome;
using YksMC.MinecraftModel.Block;
using YksMC.MinecraftModel.BlockType;
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
        private readonly IBlockTypeRepository _blockTypeRepository;
        private readonly IBiomeRepository _biomeRepository;

        public ChunkDataHandler(IPacketReader packetReader, IBlockTypeRepository blockTypeRepository, IBiomeRepository biomeRepository)
        {
            _reader = packetReader;
            _blockTypeRepository = blockTypeRepository;
            _biomeRepository = biomeRepository;
        }

        public IWorld ApplyEvent(ChunkDataPacket packet, IWorld world)
        {
            _reader.SetPacket(packet.DataAndBiomes.Values);

            IChunkCoordinate position = new ChunkCoordinate(packet.ChunkX, packet.ChunkZ);
            IChunk chunk = world.GetChunk(position);

            chunk = ParseChunk(packet, chunk, world.Dimension);

            return world.ChangeChunk(position, chunk);
        }

        private IChunk ParseChunk(ChunkDataPacket packet, IChunk chunk, IDimension dimension)
        {
            for (int sectionY = 0; sectionY < _primaryBitMaskBits; sectionY++)
            {
                int sectionBit = (packet.PrimaryBitMask >> sectionY) & 0b1;
                if (sectionBit == 0)
                {
                    continue;
                }
                chunk = ParseChunkSection(packet, chunk, sectionY, dimension);
            }
            return chunk;
        }

        private IChunk ParseChunkSection(ChunkDataPacket packet, IChunk chunk, int sectionY, IDimension dimension)
        {
            byte bitsPerBlock = GetBitsPerBlock();
            int[] typePalette = GetBlockTypePalette();
            ulong[] typeData = GetBlockTypeData();
            byte[] lightData = GetLightData(dimension);

            for (int localY = 0; localY < _sectionHeight; localY++)
            {
                for (int z = 0; z < _sectionWidth; z++)
                {
                    for (int x = 0; x < _sectionWidth; x++)
                    {
                        IBlockCoordinate position = new BlockCoordinate(x, sectionY * _sectionHeight + localY, z);
                        IBlock block = chunk.GetBlock(position);

                        block = ParseType(block, position, bitsPerBlock, typePalette, typeData);
                        block = ParseLightLevels(block, position, lightData, dimension);
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

        private byte[] GetLightData(IDimension dimension)
        {
            int bytes = _sectionHeight * _sectionWidth * _sectionWidth;
            if (!dimension.HasSkylight)
            {
                bytes /= 2;
            }
            return _reader.GetBytes(bytes);
        }

        private IBlock ParseType(IBlock block, IBlockCoordinate position, int bitsPerBlock, int[] typePalette, ulong[] typeData)
        {
            int blockIndex = (((position.Y * _sectionHeight) + position.Z) * _sectionWidth + position.X);
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
            if(typePalette.Length > 0)
            {
                paletteValue = typePalette[paletteIndex];
            }
            else
            {
                paletteValue = (int)paletteIndex;
            }

            int metadata = paletteValue & 0b1111;
            int typeId = paletteValue >> 4;
            IBlockType type = _blockTypeRepository.GetType(new BlockTypeIdentity(typeId, metadata));
            return block.ChangeType(type);
        }

        private IBlock ParseLightLevels(IBlock block, IBlockCoordinate position, byte[] lightData, IDimension dimension)
        {
            int lightIndex = ((position.Y * _sectionHeight) + position.Z) * _sectionWidth + position.X;
            int lightBitOffset = 4 * (lightIndex % 2);
            int lightLevel = (lightData[lightIndex / 2] >> lightBitOffset) & 0b1111;

            if (!dimension.HasSkylight)
            {
                return block.ChangeLightLevel(new LightLevel(lightLevel));
            }

            int skylightOffset = (_sectionHeight * _sectionWidth * _sectionWidth) / 2;
            int skylightLevel = (lightData[skylightOffset + lightIndex / 2] >> lightBitOffset) & 0b1111;

            return block.ChangeLightLevels(new LightLevel(lightLevel), new LightLevel(skylightLevel));
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
