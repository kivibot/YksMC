using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.EntityType
{
    public interface IEntityTypeRepository
    {
        IEntityType GetPlayerType();
    }
}
