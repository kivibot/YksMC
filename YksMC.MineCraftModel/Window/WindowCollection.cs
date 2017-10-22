using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.Common;

namespace YksMC.MinecraftModel.Window
{
    public class WindowCollection : IWindowCollection
    {
        private readonly IImmutableDictionary<int, IWindow> _windows;

        public IWindow this[int id] => _windows[id];

        public WindowCollection()
            : this(new ImmutableDictionary<int, IWindow>())
        {

        }

        public WindowCollection(IImmutableDictionary<int, IWindow> windows)
        {
            _windows = windows;
        }

        public IWindowCollection WithWindow(IWindow window)
        {
            IImmutableDictionary<int, IWindow> windows = _windows.WithEntry(window.Id, window);
            return new WindowCollection(windows);
        }

        public IWindow GetNewestWindow()
        {
            //TODO: implement correctly
            return _windows.Values.OrderByDescending(w => w.Id).First();
        }

        public bool ContainsWindow(int id)
        {
            return _windows.ContainsKey(id);
        }

        public IWindowCollection WithoutWindow(int windowId)
        {
            IImmutableDictionary<int, IWindow> windows = _windows.WithoutEntry(windowId);
            return new WindowCollection(windows);
        }
    }
}
