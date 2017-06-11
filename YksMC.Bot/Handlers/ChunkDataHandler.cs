using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YksMC.Abstraction.Bot;
using YksMC.Abstraction.Chunk;
using YksMC.Abstraction.World;
using YksMC.Client.EventBus;
using YksMC.Protocol;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Bot.Handlers
{
    public class ChunkDataHandler : IEventHandler<ChunkDataPacket>
    {
        private const int _chunkWidth = 16;
        private const int _chunkSectionHeight = 16;

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
                chunk = _chunkService.CreateChunk(_bot.Player.Dimension, packet.ChunkX, packet.ChunkZ);
            }
            else
            {
                chunk = _chunkService.GetChunk(_bot.Player.Dimension, packet.ChunkX, packet.ChunkZ);
            }

            for (int i = 0; i < 32; i++)
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

            for (int z = 0; z < _chunkWidth; z++)
            {
                for (int x = 0; x < _chunkWidth; x++)
                {
                    byte biomeId = _packetReader.GetByte();
                    //TODO: biome
                    Biome biome = (Biome)biomeId;
                    _chunkService.SetBiome(chunk.Dimension, _chunkWidth * chunk.X + x, _chunkWidth * chunk.Z + z, biome);
                }
            }
        }

        private void ParseChunkSection(IChunk chunk, int sectionY)
        {
            ParseBlockTypes(chunk, sectionY);
            ParseBlockLight(chunk, sectionY);
            ParseSkyLight(chunk, sectionY);
        }

        private void ParseBlockTypes(IChunk chunk, int sectionY)
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
                        BlockLocation location = new BlockLocation(chunk.Dimension, chunk.X * _chunkWidth + x, sectionY * _chunkSectionHeight + y, chunk.Z * _chunkWidth + z);
                        _chunkService.SetBlockType(location, blockType);

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

        private void ParseBlockLight(IChunk chunk, int sectionY)
        {
            for (int y = 0; y < _chunkSectionHeight; y++)
            {
                for (int z = 0; z < _chunkWidth; z++)
                {
                    for (int x = 0; x < _chunkWidth; x += 2)
                    {
                        byte value = _packetReader.GetByte();
                        BlockLocation firstLocation = new BlockLocation(chunk.Dimension, chunk.X * _chunkWidth + x, sectionY * _chunkSectionHeight + y, chunk.Z * _chunkWidth + z);
                        BlockLocation secondLocation = new BlockLocation(chunk.Dimension, chunk.X * _chunkWidth + x + 1, sectionY * _chunkSectionHeight + y, chunk.Z * _chunkWidth + z);
                        _chunkService.SetBlockLight(firstLocation, (byte)(value & 0b1111));
                        _chunkService.SetBlockLight(secondLocation, (byte)(value >> 4));
                    }
                }
            }
        }

        private void ParseSkyLight(IChunk chunk, int sectionY)
        {
            if (chunk.Dimension != Dimension.Overworld)
                return;

            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int x = 0; x < 16; x += 2)
                    {
                        byte value = _packetReader.GetByte();
                        BlockLocation firstLocation = new BlockLocation(chunk.Dimension, chunk.X * 16 + x, sectionY * 16 + y, chunk.Z * 16 + z);
                        BlockLocation secondLocation = new BlockLocation(chunk.Dimension, chunk.X * 16 + x + 1, sectionY * 16 + y, chunk.Z * 16 + z);
                        _chunkService.SetSkyLight(firstLocation, (byte)(value & 0b1111));
                        _chunkService.SetSkyLight(secondLocation, (byte)(value >> 4));
                    }
                }
            }
        }
    }
}
