using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Inventory;

namespace YksMC.MinecraftModel.Player
{
    public interface IPlayer
    {
        Guid Id { get; }
        int EntityId { get; }
        int DimensionId { get; }
        bool HasEntity { get; }
        string Name { get; }
        
        int Level { get; }
        double LevelProgress { get; }
        int TotalExperiece { get; }

        IPlayer ChangeEntity(int entityId, int dimensionId);

        IPlayer ChangeExperience(int level, double levelProgress, int totalExperience);

        IPlayerInventory GetInventory();
        IPlayer ChangeInvetory(IPlayerInventory playerInventory);
    }
}
