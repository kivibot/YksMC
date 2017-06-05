using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Models;

namespace YksMC.Bot.Services
{
    public interface IBlockTypeService
    {
        BlockType GetBlockType(int id, int data);
        BlockType GetAirBlock();
    }
}
