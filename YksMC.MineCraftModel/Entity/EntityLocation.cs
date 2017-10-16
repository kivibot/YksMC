using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Common;

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

        public IEntityLocation Add(IVector3<double> vector)
        {
            return new EntityLocation(_x + vector.X, _y + vector.Y, _z + vector.Z);
        }

        public IVector3<double> AsVector()
        {
            return new Vector3d(_x, _y, _z);
        }

        public static readonly IEntityLocation Origin = new EntityLocation(0, 0, 0);
    }
}
