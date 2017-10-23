using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Window
{
    public interface IWindowCollection
    {
        IWindow this[int id] { get; }

        IWindowCollection WithWindow(IWindow window);
        IWindowCollection WithoutWindow(int windowId);

        IWindow GetNewestWindow();
        IWindow GetCursorWindow();

        bool ContainsWindow(int id);
    }
}
