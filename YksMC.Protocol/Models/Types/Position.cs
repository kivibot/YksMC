using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;

namespace YksMC.Protocol.Models.Types
{
    public sealed class Position
    {
        [Ignored]
        public int X { get; }
        [Ignored]
        public int Y { get; }
        [Ignored]
        public int Z { get; }
        public ulong Value { get; }

        public Position(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        [DefaultConstructor]
        public Position(ulong value)
        {
            int x = (int)(value >> 38);
            int y = (int)((value >> 26) & 0xFFF);
            int z = (int)((value << 38) >> 38);

            X = x >> 25 == 0 ? x : x - (1 << 26);
            Y = y >> 11 == 0 ? y : y - (1 << 12);
            Z = z >> 25 == 0 ? z : z - (1 << 26);
        }

        public override bool Equals(object obj)
        {
            Position other = obj as Position;
            if (other == null)
                return false;
            if (X != other.X)
                return false;
            if (Y != other.Y)
                return false;
            if (Z != other.Z)
                return false;
            return true;
        }
    }
}
