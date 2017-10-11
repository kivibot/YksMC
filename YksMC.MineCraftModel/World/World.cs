using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.Chunk;

namespace YksMC.MinecraftModel.World
{
    public class World : IWorld
    {
        private readonly IDimension _dimension;
        private readonly IChunk _emptyChunk;
        private readonly IReadOnlyDictionary<IChunkCoordinate, IChunk> _chunks;

        public IDimension Dimension => _dimension;

        public World(IDimension dimension, IChunk emptyChunk)
            : this(dimension, emptyChunk, new Dictionary<IChunkCoordinate, IChunk>())
        {
        }

        public World(IDimension dimension, IChunk emptyChunk, IReadOnlyDictionary<IChunkCoordinate, IChunk> chunks)
        {
            _dimension = dimension;
            _emptyChunk = emptyChunk;
            _chunks = chunks;
        }

        public IWorld ChangeChunk(IChunkCoordinate position, IChunk chunk)
        {
            Dictionary<IChunkCoordinate, IChunk> chunks = _chunks.ToDictionary(e => e.Key, e => e.Value);
            chunks[position] = chunk;
            return new World(_dimension, _emptyChunk, chunks);
        }

        public IChunk GetChunk(IChunkCoordinate position)
        {
            if (_chunks.TryGetValue(position, out IChunk chunk))
            {
                return chunk;
            }
            return _emptyChunk;
        }
    }
}
