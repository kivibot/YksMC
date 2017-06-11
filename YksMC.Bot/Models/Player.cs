using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Misc;
using YksMC.Abstraction.World;

namespace YksMC.Bot.Models
{
    public class Player : IPlayer
    {
        public int EntityId { get; set; }
        public Guid UserId { get; set; }
        public Dimension Dimension { get; set; }
        public Vector3<double> Position { get; set; }
        public LookDirection LookDirection { get; set; }
        public bool IsOnGround { get; set; }

    }
}
