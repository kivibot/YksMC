using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.MinecraftModel.Chunk
{
    public class Chunk : IChunk
    {
        public const int Width = ChunkSection.Width;
        private const int _sectionsCount = 32;
        public const int Height = _sectionsCount * ChunkSection.Height;

        private readonly ChunkSection[] _sections;

        public Chunk(IBlock emptyBlock)
        {
            ChunkSection[] sections = new ChunkSection[_sectionsCount];
            ChunkSection emptySection = new ChunkSection(emptyBlock);
            for (int i = 0; i < sections.Length; i++)
            {
                sections[i] = emptySection;
            }
            _sections = sections;
        }

        public Chunk(ChunkSection[] sections)
        {
            if (sections.Length * ChunkSection.Height != Height)
            {
                throw new ArgumentException($"Invalid section count: {sections.Length}");
            }
            _sections = sections;
        }

        public IChunk ChangeBlock(IBlockLocation position, IBlock block)
        {
            ChunkSection[] sections = (ChunkSection[])_sections.Clone();
            int sectionY = position.Y / ChunkSection.Height;
            sections[sectionY] = sections[sectionY].ChangeBlock(position, block);
            return new Chunk(sections);
        }

        public T GetBlock<T>(IBlockLocation position) where T : class, IBlock
        {
            ChunkSection section = GetSection(position);
            return section.GetBlock(position) as T;
        }

        private ChunkSection GetSection(IBlockLocation position)
        {
            return _sections[position.Y / ChunkSection.Height];
        }
    }
}
