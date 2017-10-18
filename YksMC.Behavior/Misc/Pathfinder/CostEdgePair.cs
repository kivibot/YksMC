using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Behavior.Misc.Pathfinder
{
    internal class CostEdgePair<TEdge>
    {
        public double Cost { get; set; }
        public TEdge Edge { get; set; }
    }
}
