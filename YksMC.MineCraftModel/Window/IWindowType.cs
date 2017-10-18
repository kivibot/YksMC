using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Window
{
    public interface IWindowType
    {
        bool ContainsPlayerInventory { get; }
        int PlayerInventoryOffset { get; }
        int HotbarOffset { get; }
    }
}
