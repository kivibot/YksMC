using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Behavior.Misc.Pathfinder
{
    internal interface IPathfinderDatastructure<TVertex, TEdge>
    {
        bool IsEmpty { get; }

        void AddOrUpdateIfSmaller(double cost, TVertex vertex, TEdge edge);
        CostEdgePair<TEdge> PopFirst();
        
        bool IsVisited(TVertex vertex);
        TEdge GetVisitEdge(TVertex vertex);
    }
}
