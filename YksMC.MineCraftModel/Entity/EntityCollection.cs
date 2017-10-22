using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Common;

namespace YksMC.MinecraftModel.Entity
{
    public class EntityCollection : IEntityCollection
    {
        private readonly IImmutableDictionary<int, IEntity> _entities;

        public IEntity this[int id] => _entities[id];

        public EntityCollection()
            :this(new ImmutableDictionary<int, IEntity>())
        {

        }

        private EntityCollection(IImmutableDictionary<int, IEntity> entities)
        {
            _entities = entities;
        }

        public IEntityCollection ReplaceEntity(IEntity entity)
        {
            IImmutableDictionary<int, IEntity> entities = _entities.WithEntry(entity.Id, entity);
            return new EntityCollection(entities);
        }

        public bool TryGetEntity(int id, out IEntity entity)
        {
            return _entities.TryGetValue(id, out entity);
        }
    }
}
