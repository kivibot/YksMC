using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.EntityType;

namespace YksMC.MinecraftModel.Entity
{
    public interface IEntity
    {
        int Id { get; }
        IEntityType Type { get; }
        IEntityCoordinate Position { get; }

        IEntity ChangePosition(IEntityCoordinate position);
    }
}
