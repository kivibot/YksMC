using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Misc;
using YksMC.Abstraction.World;

namespace YksMC.Bot.Models
{
    public class Player
    {
        public int EntityId { get; private set; }
        public Guid UserId { get; private set; }
        public Dimension Dimension { get; private set; }
        public Vector3<double> Position { get; private set; }
        public LookDirection LookDirection { get; private set; }
        public bool IsOnGround { get; private set; }

        public Player(int entityId, Guid userId)
        {
            EntityId = entityId;
            UserId = userId;
            Position = Vector3<double>.Zero;
        }

        public void ChangeLookDirection(LookDirection value)
        {
            LookDirection = value;
        }

        public void MoveTo(Vector3<double> position)
        {
            Position = position;
        }

        public void SpawnTo(Dimension dimension)
        {
            Dimension = dimension;
        }
    }
}
