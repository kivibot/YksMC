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
        private readonly bool _isDiggable;
        private readonly double _hardness;

        public string Name => _name;
        public bool IsSolid => _isSolid;
        public bool IsDiggable => _isDiggable;
        public double Hardness => _hardness;

        public BlockType(string name, bool isSolid, bool isDiggable, double hardness)
        {
            _name = name;
            _isSolid = isSolid;
            _isDiggable = isDiggable;
            _hardness = hardness;
        }
    }
}
