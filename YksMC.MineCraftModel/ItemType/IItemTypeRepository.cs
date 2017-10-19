using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.ItemType
{
    [Obsolete("Replaced by itemtack")]
    public interface IItemTypeRepository
    {
        IItemType Get(int id);
    }
}
