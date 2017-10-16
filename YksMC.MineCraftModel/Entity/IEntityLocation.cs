using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Common;

namespace YksMC.MinecraftModel.Entity
{
    public interface IEntityLocation
    {
        double X { get; }
        double Y { get; }
        double Z { get; }

        IEntityLocation Add(IVector3<double> vector);
        IVector3<double> AsVector();
    }
}
