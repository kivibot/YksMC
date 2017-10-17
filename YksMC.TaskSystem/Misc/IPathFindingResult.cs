using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Block;

namespace YksMC.Behavior.Misc
{
    public interface IPathFindingResult
    {
        bool Failed { get; }
        IReadOnlyList<IBlockLocation> Path { get; }
    }
}
