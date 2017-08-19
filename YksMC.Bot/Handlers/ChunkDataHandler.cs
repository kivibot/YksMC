﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YksMC.Abstraction.Chunk.Models;
using YksMC.Abstraction.Chunk.Services;
using YksMC.Abstraction.World.Models;
using YksMC.Client.EventBus;
using YksMC.Protocol;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Bot.Handlers
{
    public class ChunkDataHandler : IEventHandler<ChunkDataPacket>
    {
        private const int _chunkWidth = 16;
        private const int _chunkSectionHeight = 16;
        private const int _chunkSections = 32;
        private const int _chunkSectionVolume = _chunkWidth * _chunkWidth * _chunkSectionHeight;
        private readonly LightLevel _defaultSkylightLevel = 0;

        private readonly IPacketReader _packetReader;
        private readonly IChunkService _chunkService;
        private readonly IMinecraftBot _bot;
        private readonly IBlockTypeService _blockTypeService;

        public ChunkDataHandler(IPacketReader reader, IChunkService chunkService, IMinecraftBot bot, IBlockTypeService typeService)
        {
            _packetReader = reader;
            _chunkService = chunkService;
            _bot = bot;
            _blockTypeService = typeService;
        }

        public void Handle(ChunkDataPacket packet)
        {
            ParseChunks(packet);
        }

        private void ParseChunks(ChunkDataPacket packet)
        {
            _packetReader.SetPacket(packet.DataAndBiomes.Values);

            IChunk chunk;
            if (packet.GroundUpContinuous)
            {
                chunk = _chunkService.CreateChunk(_bot.Player.Dimension, new ChunkCoordinate(packet.ChunkX, packet.ChunkZ));
            }
            else
            {
                chunk = _chunkService.GetChunk(_bot.Player.Dimension, new ChunkCoordinate(packet.ChunkX, packet.ChunkZ));
            }

            for (int i = 0; i < _chunkSections; i++)
            {
                if (((packet.PrimaryBitMask >> i) & 1) == 0)
                {
                    continue;
                }
                ParseChunkSection(chunk, i);
            }

            ParseBiomes(packet, chunk);
        }

        private void ParseBiomes(ChunkDataPacket packet, IChunk chunk)
        {
            if (!packet.GroundUpContinuous)
            {
                return;
            }

            //for (int z = 0; z < _chunkWidth; z++)
            //{
            //    for (int x = 0; x < _chunkWidth; x++)
            //    {
            //        byte biomeId = _packetReader.GetByte();
            //        //TODO: biome
            //        Biome biome = (Biome)biomeId;
            //        _chunkService.SetBiome(chunk.Dimension, _chunkWidth * chunk.X + x, _chunkWidth * chunk.Z + z, biome);
            //    }
            //}
        }

        private void ParseChunkSection(IChunk chunk, int sectionY)
        {
            byte bitsPerBlock = GetBitsPerBlock();
            int[] typePalette = GetBlockTypePalette();
            ulong[] typeData = GetBlockTypeData();
            byte[] lightData = GetLightData(chunk);

            for (int y = 0; y < _chunkWidth; y++)
            {
                for (int z = 0; z < _chunkWidth; z++)
                {
                    for (int x = 0; x < _chunkSectionHeight; x++)
                    {
                        BlockCoordinate position = new BlockCoordinate(x, sectionY * _chunkSectionHeight + y, z);

                        IBlockType type = GetBlockType(typePalette, typeData, x, y, z);
                        LightLevel lightLevel = GetLightLevel(lightData, false, x, y, z);
                        LightLevel skylightLevel = chunk.World.HasSkylight ? GetLightLevel(lightData, true, x, y, z) : _defaultSkylightLevel;

                        chunk.ChangeBlock(position, type, lightLevel, skylightLevel);
                    }
                }
            }
        }

        private byte GetBitsPerBlock()
        {
            byte bitsPerBlock = _packetReader.GetByte();
            return bitsPerBlock;
        }

        private int[] GetBlockTypePalette()
        {
            int paletteLenght = _packetReader.GetVarInt();
            int[] palette = new int[paletteLenght];
            for (int i = 0; i < paletteLenght; i++)
            {
                palette[i] = _packetReader.GetVarInt();
            }
            return palette;
        }

        private ulong[] GetBlockTypeData()
        {
            int dataLength = _packetReader.GetVarInt();
            ulong[] data = new ulong[dataLength];
            for (int i = 0; i < dataLength; i++)
            {
                data[i] = _packetReader.GetUnsignedLong();
            }
            return data;
        }

        private byte[] GetLightData(IChunk chunk)
        {
            int bytes = _chunkSectionVolume;
            if (!chunk.World.HasSkylight)
            {
                bytes /= 2;
            }
            return _packetReader.GetBytes(bytes);
        }

        private IBlockType GetBlockType(int[] typePalette, ulong[] typeData, int x, int y, int z)
        {

        }

        private LightLevel GetLightLevel(byte[] lightData, bool skylight, int x, int y, int z)
        {

        }
        
        private void ParseBlockTypes(IChunk chunk, int sectionY)
        {
            int blockIndex = 0;
            ulong dataMask = (1UL << bitsPerBlock) - 1UL;
            bool usePalette = paletteLenght > 0;

            for (int y = 0; y < _chunkWidth; y++)
            {
                for (int z = 0; z < _chunkWidth; z++)
                {
                    for (int x = 0; x < _chunkSectionHeight; x++)
                    {
                        int currentDataIndex = (blockIndex * bitsPerBlock) / 64;
                        int currentDataOffset = (blockIndex * bitsPerBlock) % 64;

                        ulong tmp = data[currentDataIndex] >> currentDataOffset;
                        if (currentDataOffset + bitsPerBlock > 64)
                        {
                            tmp |= data[currentDataIndex + 1] << 64 - currentDataOffset;
                        }
                        tmp &= dataMask;

                        int blockStateId;
                        if (usePalette)
                        {
                            blockStateId = palette[tmp];
                        }
                        else
                        {
                            blockStateId = (int)tmp;
                        }

                        IBlockType blockType = _blockTypeService.GetBlockType(blockStateId >> 4, (byte)(blockStateId & 0b1111));
                        BlockCoordinate position = new BlockCoordinate(x, sectionY * _chunkSectionHeight + y, z);
                        _chunkService.SetBlockType(position, blockType);

                        blockIndex++;
                    }
                }
            }
        }

        private void ParseBlockLight(IChunk chunk, int sectionY)
        {
            for (int y = 0; y < _chunkSectionHeight; y++)
            {
                for (int z = 0; z < _chunkWidth; z++)
                {
                    for (int x = 0; x < _chunkWidth; x += 2)
                    {
                        byte value = _packetReader.GetByte();
                        BlockCoordinate firstLocation = new BlockCoordinate(x, sectionY * _chunkSectionHeight + y, z);
                        BlockCoordinate secondLocation = new BlockCoordinate(x + 1, sectionY * _chunkSectionHeight + y, z);
                        _chunkService.SetBlockLight(firstLocation, (byte)(value & 0b1111));
                        _chunkService.SetBlockLight(secondLocation, (byte)(value >> 4));
                    }
                }
            }
        }
    }
}
