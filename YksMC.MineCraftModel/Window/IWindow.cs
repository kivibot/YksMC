using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Window
{
    public interface IWindow
    {
        int Id { get; }
        string Title { get; }
        IReadOnlyList<IWindowSlot> Slots { get; }

        IWindow ReplaceSlot(IWindowSlot slot);
    }
}
