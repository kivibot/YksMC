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

        public string Name => _name;

        public BlockType(string name)
        {
            _name = name;
        }
    }
}
