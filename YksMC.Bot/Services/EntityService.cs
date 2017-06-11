using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Misc;
using YksMC.Abstraction.World;
using YksMC.Bot.Models;

namespace YksMC.Bot.Services
{
    public class EntityService : IEntityService
    {
        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();

        public IPlayer CreatePlayer(int entityId, Guid uuid, Dimension dimension)
        {
            Player player = new Player()
            {
                EntityId = entityId,
                UserId = uuid,
                Dimension = dimension,
                Position = new Vector3<double>(0, 0, 0),
                LookDirection = new LookDirection(0, 0),
                IsOnGround = true
            };

            _players.Add(player.EntityId, player);
            return player;
        }

        public void SetPlayerLookDirection(int entityId, LookDirection lookDirection)
        {
            Player player = _players[entityId];
            player.LookDirection = lookDirection;
        }

        public void SetPlayerPosition(int entityId, Vector3<double> position)
        {
            Player player = _players[entityId];
            player.Position = position;
        }
    }
}
