using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Abstraction.Models
{
    public interface IBlockType
    {
        int Id { get; }
        int Data { get; }
        string Name { get; }
        double Hardness { get; }
        int StackSize { get; }
        bool Diggable { get; }
        BoundingBoxType BoundingBox { get; }
        bool Transparent { get; }
        int EmitLight { get; }
        int FilterLight { get; }
    }
}
