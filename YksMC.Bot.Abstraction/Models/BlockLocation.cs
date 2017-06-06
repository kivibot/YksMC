using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Abstraction.Models
{
    public struct BlockLocation
    {
        public Dimension Dimension { get; }
        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public BlockLocation(Dimension dimension, int x, int y, int z)
        {
            Dimension = dimension;
            X = x;
            Y = y;
            Z = z;
        }
    }
}
