using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Window
{
    public class WindowType : IWindowType
    {
        private readonly bool _containsPlayerInventory;
        private readonly int _playerInventoryOffset;
        private readonly int _hotbarOffset;

        public bool ContainsPlayerInventory => _containsPlayerInventory;
        public int PlayerInventoryOffset => _playerInventoryOffset;
        public int HotbarOffset => _hotbarOffset;

        public WindowType(bool containsPlayerInventory, int playerInventoryOffset, int hotbarOffset)
        {
            _containsPlayerInventory = containsPlayerInventory;
            _playerInventoryOffset = playerInventoryOffset;
            _hotbarOffset = hotbarOffset;
        }
    }
}
