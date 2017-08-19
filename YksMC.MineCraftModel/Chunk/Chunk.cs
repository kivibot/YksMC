using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.MinecraftModel.Chunk
{
    public class Chunk : IChunk
    {
        public const int Width = 16;
        public const int Height = 32 * 16;

        private readonly ChunkSection[] _sections;

        public Chunk(ChunkSection[] sections)
        {
            if (sections.Length * ChunkSection.Height != Height)
            {
                throw new ArgumentException($"Invalid section count: {sections.Length}");
            }
            _sections = sections;
        }

        public IChunk ChangeBlock(BlockCoordinate position, IBlock block)
        {
            ChunkSection[] sections = (ChunkSection[])_sections.Clone();
            int sectionY = position.Y / ChunkSection.Height;
            sections[sectionY] = sections[sectionY].ChangeBlock(position, block);
            return new Chunk(sections);
        }

        public IBlock GetBlock(BlockCoordinate position)
        {
            ChunkSection section = GetSection(position);
            return section.GetBlock(position);
        }

        private ChunkSection GetSection(BlockCoordinate position)
        {
            return _sections[position.Y / ChunkSection.Height];
        }
    }
}
