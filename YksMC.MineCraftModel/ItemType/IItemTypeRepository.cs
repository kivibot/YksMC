using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.ItemType
{
    public interface IItemTypeRepository
    {
        IItemType Get(int id);
    }
}
