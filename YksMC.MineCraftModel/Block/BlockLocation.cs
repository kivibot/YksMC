using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.Entity;

namespace YksMC.MinecraftModel.Block
{
    [DebuggerDisplay("[{X}, {Y}, {Z}]")]
    public class BlockLocation : IBlockLocation
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

        public int X => _x;
        public int Y => _y;
        public int Z => _z;

        public BlockLocation(IEntityLocation entityLocation)
        {
            _x = (int)Math.Floor(entityLocation.X);
            _y = (int)Math.Floor(entityLocation.Y);
            _z = (int)Math.Floor(entityLocation.Z);
        }

        public BlockLocation(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public IBlockLocation Add(int x, int y, int z)
        {
            return new BlockLocation(_x + x, _y + y, _z + z);
        }

        public override bool Equals(object obj)
        {
            var location = obj as IBlockLocation;
            return location != null &&
                   X == location.X &&
                   Y == location.Y &&
                   Z == location.Z;
        }

        public override int GetHashCode()
        {
            var hashCode = 1753160635;
            hashCode = hashCode * -1521134295 + _x.GetHashCode();
            hashCode = hashCode * -1521134295 + _y.GetHashCode();
            hashCode = hashCode * -1521134295 + _z.GetHashCode();
            return hashCode;
        }

        public IVector3<double> AsVector()
        {
            return new Vector3d(_x, _y, _z);
        }

        public override string ToString()
        {
            return $"{_x}, {_y}, {_z}";
        }
    }
}
