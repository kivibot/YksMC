using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Models;

namespace YksMC.Bot.Services
{
    public class BlockTypeService : IBlockTypeService
    {
        public BlockType GetAirBlock()
        {
            return new BlockType() { BlockId = 0 };
        }

        public BlockType GetBlockType(int id, int data)
        {
            return new BlockType() { BlockId = id };
        }
    }
}
