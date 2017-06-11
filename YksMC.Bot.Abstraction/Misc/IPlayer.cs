using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.World;

namespace YksMC.Abstraction.Misc
{
    public interface IPlayer
    {
        int EntityId { get; }
        Guid UserId { get; }
        Dimension Dimension { get; }

        Vector3<double> Position { get; }
        LookDirection LookDirection { get; }
        bool IsOnGround { get; }
    }
}
