using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Block
{
    public class BlockTypeInfo
    {
        private readonly string _name;
        private readonly bool _isSolid;
        private readonly bool _isDiggable;
        private readonly double _hardness;
        private readonly HarvestTier _harvestTier;
        private readonly BlockMaterial _material;
        private readonly bool _isDangerous;
        private readonly bool _isEmpty;

        public string Name => _name;
        public bool IsSolid => _isSolid;
        public bool IsDiggable => _isDiggable;
        public double Hardness => _hardness;
        public HarvestTier HarvestTier => _harvestTier;
        public BlockMaterial Material => _material;
        public bool IsDangerous => _isDangerous;
        public bool IsEmpty => _isEmpty;

        public BlockTypeInfo(string name, bool isSolid, bool isDiggable, double hardness, HarvestTier harvestTier, BlockMaterial blockMaterial, bool isDangerous, bool isEmpty)
        {
            _name = name;
            _isSolid = isSolid;
            _isDiggable = isDiggable;
            _hardness = hardness;
            _harvestTier = harvestTier;
            _material = blockMaterial;
            _isDangerous = isDangerous;
            _isEmpty = isEmpty;
        }
    }
}
