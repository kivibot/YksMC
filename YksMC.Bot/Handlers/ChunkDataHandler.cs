using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.Models;
using YksMC.Bot.Services;
using YksMC.Client.Handler;
using YksMC.Protocol;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Bot.Handlers
{
    public class ChunkDataHandler : IPacketHandler<ChunkDataPacket>
    {
        private readonly IPacketReader _packetReader;
        private readonly IChunkService _chunkService;
        private readonly IEntityService _entityService;
        private readonly IBlockTypeService _blockTypeService;

        public ChunkDataHandler(IPacketReader reader, IChunkService chunkService, IEntityService entityService, IBlockTypeService blockTypeService)
        {
            _packetReader = reader;
            _chunkService = chunkService;
            _entityService = entityService;
            _blockTypeService = blockTypeService;
        }

        public async Task HandleAsync(ChunkDataPacket packet)
        {
            ParseChunks(packet);
        }

        private void ParseChunks(ChunkDataPacket packet)
        {
            _packetReader.SetPacket(packet.DataAndBiomes.Values);

            Dimension dimension = _entityService.GetLocalPlayer().Dimension;

            Chunk chunk;
            if (packet.GroundUpContinuous)
                chunk = _chunkService.CreateChunk(dimension, packet.ChunkX, packet.ChunkZ);
            else
                chunk = _chunkService.GetChunk(dimension, packet.ChunkX, packet.ChunkZ);

            for (int i = 0; i < 32; i++)
            {
                if (((packet.PrimaryBitMask >> i) & 1) == 0)
                    continue;

                ParseChunkSection(chunk, i);
            }

            ParseBiomes(packet, chunk);
        }

        private void ParseBiomes(ChunkDataPacket packet, Chunk chunk)
        {
            if (!packet.GroundUpContinuous)
                return;

            for (int z = 0; z < ChunkSection.Width; z++)
            {
                for (int x = 0; x < ChunkSection.Width; x++)
                {
                    byte biomeId = _packetReader.GetByte();
                    Biome biome = (Biome)biomeId;
                    _chunkService.SetBiome(chunk, x, z, biome);
                }
            }
        }

        private void ParseChunkSection(Chunk chunk, int sectionY)
        {
            ParseBlockTypes(chunk, sectionY);
            ParseBlockLight(chunk, sectionY);
            ParseSkyLight(chunk, sectionY);
        }

        private void ParseBlockTypes(Chunk chunk, int sectionY)
        {
            byte bitsPerBlock = _packetReader.GetByte();
            bitsPerBlock = GetRealBitsPerBlock(bitsPerBlock);

            int paletteLenght = _packetReader.GetVarInt();
            int[] palette = new int[paletteLenght];
            for (int i = 0; i < paletteLenght; i++)
                palette[i] = _packetReader.GetVarInt();

            int dataLength = _packetReader.GetVarInt();
            ulong[] data = new ulong[dataLength];
            for (int i = 0; i < dataLength; i++)
                data[i] = _packetReader.GetUnsignedLong();

            int blockIndex = 0;
            ulong dataMask = (1UL << bitsPerBlock) - 1UL;
            bool usePalette = paletteLenght > 0;

            for (int y = 0; y < ChunkSection.Height; y++)
            {
                for (int z = 0; z < ChunkSection.Width; z++)
                {
                    for (int x = 0; x < ChunkSection.Width; x++)
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
                            blockStateId = palette[tmp];
                        else
                            blockStateId = (int)tmp;

                        BlockType blockType = _blockTypeService.GetBlockType(blockStateId >> 4, blockStateId & 0b1111);
                        _chunkService.SetBlockType(chunk, x, sectionY * ChunkSection.Height + y, z, blockType);

                        blockIndex++;
                    }
                }
            }
        }

        private byte GetRealBitsPerBlock(byte val)
        {
            if (val <= 4)
                return 4;
            if (val >= 9)
                return 13;
            return val;
        }

        private void ParseBlockLight(Chunk chunk, int sectionY)
        {
            for (int y = 0; y < ChunkSection.Height; y++)
            {
                for (int z = 0; z < ChunkSection.Width; z++)
                {
                    for (int x = 0; x < ChunkSection.Width; x += 2)
                    {
                        byte value = _packetReader.GetByte();
                        _chunkService.SetBlockLight(chunk, x, ChunkSection.Height * sectionY + y, z, (byte)(value & 0b1111));
                        _chunkService.SetBlockLight(chunk, x, ChunkSection.Height * sectionY + y, z, (byte)(value >> 4));
                    }
                }
            }
        }

        private void ParseSkyLight(Chunk chunk, int sectionY)
        {
            if (chunk.Dimension != Dimension.Overworld)
                return;

            for (int y = 0; y < ChunkSection.Height; y++)
            {
                for (int z = 0; z < ChunkSection.Width; z++)
                {
                    for (int x = 0; x < ChunkSection.Width; x += 2)
                    {
                        byte value = _packetReader.GetByte();
                        _chunkService.SetSkyLight(chunk, x, ChunkSection.Height * sectionY + y, z, (byte)(value & 0b1111));
                        _chunkService.SetSkyLight(chunk, x, ChunkSection.Height * sectionY + y, z, (byte)(value >> 4));
                    }
                }
            }
        }
    }
}
