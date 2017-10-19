using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.ItemType;

namespace YksMC.Data.Json.ItemType
{
    public class JsonItemTypeRepository : IItemTypeRepository
    {
        public IItemType Get(int id)
        {
            return new MinecraftModel.ItemType.ItemType();
        }
    }
}
