using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.BlockType
{
    public interface IBlockTypeIdentity
    {
        int Id { get; }
        int Metadata { get; }
    }
}
