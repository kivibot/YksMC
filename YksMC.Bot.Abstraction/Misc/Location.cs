using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.World;

namespace YksMC.Abstraction.Misc
{
    public class Location
    {
        public Dimension Dimension { get; }
        public Vector3<double> Position { get; set; }

        public Location(Dimension dimension, double x, double y, double z)
        {
            Dimension = dimension;
            Position = new Vector3<double>(x, y, z);
        }
    }
}
