using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Player
{
    public class Player : IPlayer
    {
        private readonly IPlayerId _id;
        private readonly int? _entityId;
        private readonly int? _dimensionId;
        private readonly string _name;
        private readonly ExperienceInfo _experienceInfo;

        public IPlayerId Id => _id;
        public int EntityId => _entityId.Value;
        public int DimensionId => _dimensionId.Value;
        public bool HasEntity => _entityId.HasValue;
        public string Name => _name;

        public int Level => _experienceInfo.Level;
        public double LevelProgress => _experienceInfo.LevelProgress;
        public int TotalExperiece => _experienceInfo.TotalExperience;
        
        public Player(IPlayerId id, string name)
            : this(id, name, null, null, new ExperienceInfo(0, 0, 0))
        {
        }

        private Player(IPlayerId id, string name, int? entityId, int? dimensionId, ExperienceInfo experienceInfo)
        {
            _id = id;
            _name = name;
            _entityId = entityId;
            _dimensionId = dimensionId;
            _experienceInfo = experienceInfo;
        }

        public IPlayer ChangeEntity(int entityId, int dimensionId)
        {
            return new Player(_id, _name, entityId, dimensionId, _experienceInfo);
        }

        public IPlayer ChangeExperience(int level, double levelProgress, int totalExperience)
        {
            return new Player(_id, _name, _entityId, _dimensionId, new ExperienceInfo(level, levelProgress, totalExperience));
        }
    }
}
