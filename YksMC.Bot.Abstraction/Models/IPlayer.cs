using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Abstraction.Models
{
    public interface IPlayer
    {
        int EntityId { get; }
        Dimension Dimension { get; }
        int Gamemode { get; }

        Vector3<double> Position { get; }
        LookDirection LookDirection { get; }
    }
}
