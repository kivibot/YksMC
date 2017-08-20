using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.BlockType
{
    public class BlockTypeIdentity : IBlockTypeIdentity
    {
        private readonly int _id;
        private readonly int _metadata;

        public int Id => _id;
        public int Metadata => _metadata;

        public BlockTypeIdentity(int id, int metadata)
        {
            _id = id;
            _metadata = metadata;
        }
    }
}
