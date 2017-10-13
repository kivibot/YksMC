﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;

namespace YksMC.MinecraftModel.Dimension
{
    public class Dimension : IDimension
    {
        private readonly int _id;
        private readonly IDimensionType _type;
        private readonly IChunk _emptyChunk;
        private readonly IReadOnlyDictionary<IChunkCoordinate, IChunk> _chunks;
        private readonly IReadOnlyDictionary<int, IEntity> _entities;
        private readonly IAgeTime _ageAndTime;

        public int Id => _id;
        public IDimensionType Type => _type;
        public IAgeTime AgeAndTime => _ageAndTime;

        public Dimension(int id, IDimensionType type, IChunk emptyChunk)
            : this(id, type, emptyChunk, new Dictionary<IChunkCoordinate, IChunk>(),
                  new Dictionary<int, IEntity>(), new AgeTime(0, 0, 24000))
        {
        }

        public Dimension(int id, IDimensionType type, IChunk emptyChunk, IReadOnlyDictionary<IChunkCoordinate, IChunk> chunks,
            IReadOnlyDictionary<int, IEntity> entities, IAgeTime ageAndTime)
        {
            _id = id;
            _type = type;
            _emptyChunk = emptyChunk;
            _chunks = chunks;
            _entities = entities;
            _ageAndTime = ageAndTime;
        }

        public IDimension ChangeChunk(IChunkCoordinate position, IChunk chunk)
        {
            Dictionary<IChunkCoordinate, IChunk> chunks = _chunks.ToDictionary(e => e.Key, e => e.Value);
            chunks[position] = chunk;
            return new Dimension(_id, _type, _emptyChunk, chunks, _entities, _ageAndTime);
        }

        public IChunk GetChunk(IChunkCoordinate position)
        {
            if (_chunks.TryGetValue(position, out IChunk chunk))
            {
                return chunk;
            }
            return _emptyChunk;
        }

        public IEntity GetEntity(int id)
        {
            return _entities[id];
        }

        public IDimension ChangeEntity(IEntity entity)
        {
            Dictionary<int, IEntity> entities = _entities.ToDictionary(e => e.Key, e => e.Value);
            entities[entity.Id] = entity;
            return new Dimension(_id, _type, _emptyChunk, _chunks, entities, _ageAndTime);
        }

        public IDimension ChangeAgeAndTime(IAgeTime ageAndTime)
        {
            return new Dimension(_id, _type, _emptyChunk, _chunks, _entities, ageAndTime);
        }
    }
}
