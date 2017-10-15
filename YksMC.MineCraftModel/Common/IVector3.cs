﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Common
{
    public interface IVector3<T>
    {
        T X { get; }
        T Y { get; }
        T Z { get; }

        IVector3<T> Multiply(T scalar);
        IVector3<T> Add(IVector3<T> vector);
    }
}
