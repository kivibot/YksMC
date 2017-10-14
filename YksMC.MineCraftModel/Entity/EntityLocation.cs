using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Entity
{
    public class EntityLocation : IEntityLocation
    {
        private readonly double _x;
        private readonly double _y;
        private readonly double _z;

        public double X => _x;
        public double Y => _y;
        public double Z => _z;

        public EntityLocation(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public static readonly IEntityLocation Origin = new EntityLocation(0, 0, 0);
    }
}
