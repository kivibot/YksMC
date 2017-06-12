using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Bot.Models
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

        public static readonly Vector3<T> Zero = new Vector3<T>(default(T), default(T), default(T));
    }
}
