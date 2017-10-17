using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Entity
{
    public interface IEntityCollection
    {
        IEntity this[int id] { get; }

        bool TryGetEntity(int id, out IEntity entity);

        IEntityCollection ReplaceEntity(IEntity entity);
    }
}
