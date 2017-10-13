using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Player
{
    public interface IPlayer
    {
        IPlayerId Id { get; }
        int EntityId { get; }
        bool HasEntity { get; }
        string Name { get; }
        
        int Level { get; }
        double LevelProgress { get; }
        int TotalExperiece { get; }

        IPlayer ChangeEntity(int entityId);

        IPlayer ChangeExperience(int level, double levelProgress, int totalExperience);
    }
}
