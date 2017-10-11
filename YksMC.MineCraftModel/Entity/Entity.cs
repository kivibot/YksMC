using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.EntityType;

namespace YksMC.MinecraftModel.Entity
{
    public class Entity : IEntity
    {
        private readonly int _id;
        private readonly IEntityType _type;
        private readonly IEntityCoordinate _position;

        public int Id => _id;
        public IEntityType Type => _type;
        public IEntityCoordinate Position => _position;

        public Entity(int id, IEntityType type, IEntityCoordinate position)
        {
            _id = id;
            _type = type;
            _position = position;
        }

        public IEntity ChangePosition(IEntityCoordinate position)
        {
            return new Entity(_id, _type, position);
        }
    }
}
