using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Common
{
    public class Vector3d : IVector3<double>
    {
        private readonly double _x;
        private readonly double _y;
        private readonly double _z;

        public double X => _x;
        public double Y => _y;
        public double Z => _z;

        public Vector3d(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public IVector3<double> Multiply(double scalar)
        {
            return new Vector3d(scalar * _x, scalar * _y, scalar * _z);
        }

        public IVector3<double> Add(IVector3<double> vector)
        {
            return new Vector3d(_x + vector.X, _y + vector.Y, _z + vector.Z);
        }

        public double Length()
        {
            return Math.Sqrt(_x * _x + _y * _y + _z * _z);
        }

        public IVector3<double> Normalize()
        {
            return Multiply(1 / Length());
        }

        public IVector3<double> Substract(IVector3<double> vector)
        {
            return new Vector3d(_x - vector.X, _y - vector.Y, _z - vector.Z);
        }
    }
}
