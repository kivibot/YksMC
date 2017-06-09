using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Abstraction.Misc
{
    public class Location
    {
        public Dimension Dimension { get; }
        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        public Location(Dimension dimension, double x, double y, double z)
        {
            Dimension = dimension;
            X = x;
            Y = y;
            Z = z;
        }
    }
}
