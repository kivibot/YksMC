using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Abstraction.Chunk
{
    public interface IBlockType
    {
        int Id { get; }
        int Data { get; }
        string Name { get; }
        double Hardness { get; }
        int StackSize { get; }
        bool IsDiggable { get; }
        BoundingBoxType BoundingBox { get; }
        bool IsTransparent { get; }
        int EmitLight { get; }
        int FilterLight { get; }
    }
}
