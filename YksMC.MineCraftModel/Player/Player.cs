using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Inventory;

namespace YksMC.MinecraftModel.Player
{
    public class Player : IPlayer
    {
        private readonly Guid _id;
        private readonly int? _entityId;
        private readonly int? _dimensionId;
        private readonly string _name;
        private readonly ExperienceInfo _experienceInfo;
        private readonly IPlayerInventory _inventory;

        public Guid Id => _id;
        public int EntityId => _entityId.Value;
        public int DimensionId => _dimensionId.Value;
        public bool HasEntity => _entityId.HasValue;
        public string Name => _name;

        public int Level => _experienceInfo.Level;
        public double LevelProgress => _experienceInfo.LevelProgress;
        public int TotalExperiece => _experienceInfo.TotalExperience;
        
        public Player(Guid id, string name)
            : this(id, name, null, null, new ExperienceInfo(0, 0, 0), new PlayerInventory())
        {
        }

        private Player(Guid id, string name, int? entityId, int? dimensionId, ExperienceInfo experienceInfo, IPlayerInventory inventory)
        {
            _id = id;
            _name = name;
            _entityId = entityId;
            _dimensionId = dimensionId;
            _experienceInfo = experienceInfo;
            _inventory = inventory;
        }

        public IPlayer ChangeEntity(int entityId, int dimensionId)
        {
            return new Player(_id, _name, entityId, dimensionId, _experienceInfo, _inventory);
        }

        public IPlayer ChangeExperience(int level, double levelProgress, int totalExperience)
        {
            return new Player(_id, _name, _entityId, _dimensionId, new ExperienceInfo(level, levelProgress, totalExperience), _inventory);
        }

        public IPlayerInventory GetInventory()
        {
            return _inventory;
        }

        public IPlayer ChangeInvetory(IPlayerInventory playerInventory)
        {
            return new Player(_id, _name, _entityId, _dimensionId, _experienceInfo, playerInventory);
        }
    }
}
