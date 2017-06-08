using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Models;
using YksMC.Abstraction.Services;
using YksMC.Bot.Models;

namespace YksMC.Bot.Services
{
    public class EntityService : IEntityService
    {
        private Player _player;

        public IPlayer CreatePlayer(int entityId, Dimension dimension, int gamemode)
        {
            if(_player != null)
            {
                throw new ArgumentException("The player already exists!");
            }
            _player = new Player()
            {
                EntityId = entityId,
                Dimension = dimension,
                Gamemode = gamemode
            };
            return _player;
        }

        public IPlayer GetPlayer()
        {
            if (_player == null)
            {
                throw new InvalidOperationException("The player has not been created!");
            }
            return _player;
        }

        public void UpdatePlayerLookDirection(LookDirection lookDirection)
        {
            if (_player == null)
            {
                throw new InvalidOperationException("The player has not been created!");
            }
            _player.LookDirection = lookDirection;
        }

        public void UpdatePlayerPosition(Vector3<double> position)
        {
            if (_player == null)
            {
                throw new InvalidOperationException("The player has not been created!");
            }
            _player.Position = position;
        }
    }
}
