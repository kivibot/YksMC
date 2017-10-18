using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YksMC.Behavior.Misc.Pathfinder
{
    internal class PathfinderDatastructure<TVertex, TEdge> : IPathfinderDatastructure<TVertex, TEdge>
    {
        private readonly IDictionary<TVertex, TEdge> _visitEdges = new Dictionary<TVertex, TEdge>();
        private readonly IDictionary<TVertex, double> _costs = new Dictionary<TVertex, double>();

        private readonly SortedDictionary<Tuple<double, TVertex>, TEdge> _queue;

        public bool IsEmpty => _queue.Count == 0;

        public PathfinderDatastructure()
        {
            _queue = new SortedDictionary<Tuple<double, TVertex>, TEdge>(new QueueKeyComparer());
        }

        public void AddOrUpdateIfSmaller(double cost, TVertex vertex, TEdge edge)
        {
            if(_costs.TryGetValue(vertex, out double previousCost))
            {
                if (previousCost <= cost)
                {
                    return;
                }
                _queue.Remove(new Tuple<double, TVertex>(previousCost, vertex));
            }
            _costs[vertex] = cost;
            _queue.Add(new Tuple<double, TVertex>(cost, vertex), edge);
        }

        public TEdge GetVisitEdge(TVertex vertex)
        {
            return _visitEdges[vertex];
        }

        public bool IsVisited(TVertex vertex)
        {
            return _visitEdges.ContainsKey(vertex);
        }

        public CostEdgePair<TEdge> PopFirst()
        {
            KeyValuePair<Tuple<double, TVertex>, TEdge> entry = _queue.First();
            _queue.Remove(entry.Key);
            _costs.Remove(entry.Key.Item2);
            _visitEdges[entry.Key.Item2] = entry.Value;
            return new CostEdgePair<TEdge>()
            {
                Cost = entry.Key.Item1,
                Edge = entry.Value
            };
        }

        private class QueueKeyComparer : IComparer<Tuple<double, TVertex>>
        {
            public int Compare(Tuple<double, TVertex> x, Tuple<double, TVertex> y)
            {
                int costComparison = x.Item1.CompareTo(y.Item1);
                if(costComparison != 0)
                {
                    return costComparison;
                }
                return x.Item2.GetHashCode().CompareTo(y.Item2.GetHashCode());
            }
        }
    }
}
