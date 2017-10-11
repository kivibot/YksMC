using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Player
{
    public interface IPlayer
    {
        Guid Id { get; }
        int EntityId { get; }
        bool HasEntity { get; }
        string Name { get; }

        IPlayer ChangeEntity(int entityId);
    }
}
