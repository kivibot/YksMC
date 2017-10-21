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
        private readonly HarvestTier _harvestTier;
        private readonly BlockMaterial _material;
        private readonly bool _isDangerous;

        public string Name => _name;
        public bool IsSolid => _isSolid;
        public bool IsDiggable => _isDiggable;
        public double Hardness => _hardness;
        public HarvestTier HarvestTier => _harvestTier;
        public BlockMaterial Material => _material;
        public bool IsDangerous => _isDangerous;

        public BlockType(string name, bool isSolid, bool isDiggable, double hardness, HarvestTier harvestTier, BlockMaterial material, bool isDangerous)
        {
            _name = name;
            _isSolid = isSolid;
            _isDiggable = isDiggable;
            _hardness = hardness;
            _harvestTier = harvestTier;
            _material = material;
            _isDangerous = isDangerous;
        }
    }
}
