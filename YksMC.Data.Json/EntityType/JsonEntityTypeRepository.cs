using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.EntityType;

namespace YksMC.Data.Json.EntityType
{
    public class JsonEntityTypeRepository : IEntityTypeRepository
    {
        private readonly IEntityType _playerType;
        private readonly IReadOnlyDictionary<JsonEntityTypeType, IReadOnlyDictionary<int, IEntityType>> _types;

        public JsonEntityTypeRepository()
        {
            _playerType = new MinecraftModel.EntityType.EntityType("player");
            _types = JsonConvert.DeserializeObject<List<JsonEntityType>>(Resources.EntityTypes)
                .GroupBy(e => e.Type)
                .ToDictionary(
                    g => g.Key,
                    g => (IReadOnlyDictionary<int, IEntityType>)g.ToDictionary(
                        e => e.Id,
                        e => (IEntityType)new MinecraftModel.EntityType.EntityType(e.Name)
                    )
                );

        }

        public IEntityType GetMobType(int id)
        {
            return _types[JsonEntityTypeType.Mob][id];
        }

        public IEntityType GetPlayerType()
        {
            return _playerType;
        }
    }
}
