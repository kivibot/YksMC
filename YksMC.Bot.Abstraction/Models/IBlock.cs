using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Abstraction.Models
{
    public interface IBlock
    {
        IBlockType Type { get; }
        byte BlockLight { get; }
        byte SkyLight { get; }
    }
}
