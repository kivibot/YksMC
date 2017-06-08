using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Abstraction.Models
{
    public class Vector3<T>
    {
        public T X { get; }
        public T Y { get; }
        public T Z { get; }

        public Vector3(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
