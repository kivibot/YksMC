using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Models;

namespace YksMC.Bot.Services
{
    public class BlockTypeService : IBlockTypeService
    {
        private Dictionary<int, BlockType> _cache = new Dictionary<int, BlockType>();

        public BlockType GetAirBlock()
        {
            return GetBlockType(0, 0);
        }

        public BlockType GetBlockType(int id, int data)
        {
            BlockType type;
            if (_cache.TryGetValue(id, out type))
                return type;
            type = new BlockType() { BlockId = id };
            _cache[id] = type;
            return type;
        }
    }
}
