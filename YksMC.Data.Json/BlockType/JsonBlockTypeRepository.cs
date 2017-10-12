﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.BlockType;

namespace YksMC.Data.Json.BlockType
{
   public  class JsonBlockTypeRepository : IBlockTypeRepository
    {
        private readonly IReadOnlyDictionary<int, IBlockType> _blockTypes;

        public JsonBlockTypeRepository()
        {
            _blockTypes = JsonConvert.DeserializeObject<List<JsonBlockType>>(Resources.BlockTypes)
                .ToDictionary(bt => bt.Id, bt => (IBlockType)new MinecraftModel.BlockType.BlockType(bt.Name));
        }

        public IBlockType GetBlockType(IBlockTypeIdentity identity)
        {
            return _blockTypes[identity.Id];
        }
    }
}
