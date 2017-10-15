using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace YksMC.MinecraftModel.BlockType
{
    [DebuggerDisplay("{Name}")]
    public class BlockType : IBlockType
    {
        private readonly string _name;
        private readonly bool _isSolid;

        public string Name => _name;
        public bool IsSolid => _isSolid;

        public BlockType(string name, bool isSolid)
        {
            _name = name;
            _isSolid = isSolid;
        }
    }
}
