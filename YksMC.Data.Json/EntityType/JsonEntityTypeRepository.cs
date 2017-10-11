using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.EntityType;

namespace YksMC.Data.Json.EntityType
{
    public class JsonEntityTypeRepository : IEntityTypeRepository
    {
        private readonly IEntityType _playerType;

        public JsonEntityTypeRepository()
        {
            _playerType = new MinecraftModel.EntityType.EntityType("Player");
        }

        public IEntityType GetPlayerType()
        {
            return _playerType;
        }
    }
}
