using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.Chunk;
using YksMC.MinecraftModel.Entity;
using YksMC.MinecraftModel.Player;

namespace YksMC.MinecraftModel.World
{
    public class World : IWorld
    {
        private readonly IDimension _dimension;
        private readonly IChunk _emptyChunk;
        private readonly IReadOnlyDictionary<IChunkCoordinate, IChunk> _chunks;
        private readonly IReadOnlyDictionary<int, IEntity> _entities;
        private readonly IReadOnlyDictionary<Guid, IPlayer> _players;

        public IDimension Dimension => _dimension;

        public World(IDimension dimension, IChunk emptyChunk)
            : this(dimension, emptyChunk, new Dictionary<IChunkCoordinate, IChunk>(), new Dictionary<int, IEntity>(), new Dictionary<Guid, IPlayer>())
        {
        }

        public World(IDimension dimension, IChunk emptyChunk, IReadOnlyDictionary<IChunkCoordinate, IChunk> chunks, IReadOnlyDictionary<int, IEntity> entities, IReadOnlyDictionary<Guid, IPlayer> players)
        {
            _dimension = dimension;
            _emptyChunk = emptyChunk;
            _chunks = chunks;
            _entities = entities;
            _players = players;
        }

        public IWorld ChangeChunk(IChunkCoordinate position, IChunk chunk)
        {
            Dictionary<IChunkCoordinate, IChunk> chunks = _chunks.ToDictionary(e => e.Key, e => e.Value);
            chunks[position] = chunk;
            return new World(_dimension, _emptyChunk, chunks, _entities, _players);
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

        public IWorld ChangeEntity(IEntity entity)
        {
            Dictionary<int, IEntity> entities = _entities.ToDictionary(e => e.Key, e => e.Value);
            entities[entity.Id] = entity;
            return new World(_dimension, _emptyChunk, _chunks, entities, _players);
        }

        public IWorld ReplacePlayer(IPlayer player)
        {
            Dictionary<Guid, IPlayer> players = _players.ToDictionary(e => e.Key, e => e.Value);
            players[player.Id] = player;
            return new World(_dimension, _emptyChunk, _chunks, _entities, players);
        }

        public IEnumerable<IPlayer> GetPlayers()
        {
            return _players.Values.ToList();
        }
    }
}
