using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Window
{
    public interface IWindowCollection
    {
        IWindow this[int id] { get; }

        IWindowCollection ReplaceWindow(IWindow window);
    }
}
