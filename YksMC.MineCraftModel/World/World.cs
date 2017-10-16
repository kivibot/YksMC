using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Player;

namespace YksMC.MinecraftModel.World
{
    public class World : IWorld
    {
        private readonly IReadOnlyDictionary<Guid, IPlayer> _players;
        private readonly IPlayer _localPlayer;
        private readonly IReadOnlyDictionary<int, IDimension> _dimensions;
        private readonly IDimension _currentDimension;

        public World(IReadOnlyDictionary<Guid, IPlayer> players, IPlayer localPlayer, IReadOnlyDictionary<int, IDimension> dimensions, IDimension currentDimension)
        {
            _players = players;
            _localPlayer = localPlayer;
            _dimensions = dimensions;
            _currentDimension = currentDimension;
        }

        public IDimension GetCurrentDimension()
        {
            return _currentDimension;
        }

        public IDimension GetDimension(int id)
        {
            //TODO: custom dimensions? (server plugins/mods)
            return _dimensions[id];
        }

        public IPlayer GetLocalPlayer()
        {
            return _localPlayer;
        }

        public IEnumerable<IPlayer> GetPlayers()
        {
            return _players.Values;
        }

        public IWorld ReplaceCurrentDimension(IDimension dimension)
        {
            if (dimension == null)
            {
                throw new ArgumentNullException(nameof(dimension));
            }
            Dictionary<int, IDimension> dimensions = _dimensions.ToDictionary(e => e.Key, e => e.Value);
            dimensions[dimension.Id] = dimension;
            return new World(_players, _localPlayer, dimensions, dimension);
        }

        public IWorld ReplaceDimension(IDimension dimension)
        {
            if(dimension == null)
            {
                throw new ArgumentNullException(nameof(dimension));
            }
            if(_currentDimension?.Id == dimension.Id)
            {
                return ReplaceCurrentDimension(dimension);
            }

            Dictionary<int, IDimension> dimensions = _dimensions.ToDictionary(e => e.Key, e => e.Value);
            dimensions[dimension.Id] = dimension;
            return new World(_players, _localPlayer, dimensions, _currentDimension);
        }

        public IWorld ReplaceLocalPlayer(IPlayer player)
        {
            Dictionary<Guid, IPlayer> players = _players.ToDictionary(e => e.Key, e => e.Value);
            players[player.Id] = player;
            return new World(players, player, _dimensions, _currentDimension);
        }

        public IWorld ReplacePlayer(IPlayer player)
        {
            if(_localPlayer?.Id == player.Id)
            {
                return ReplaceLocalPlayer(player);
            }

            Dictionary<Guid, IPlayer> players = _players.ToDictionary(e => e.Key, e => e.Value);
            players[player.Id] = player;
            return new World(players, _localPlayer, _dimensions, _currentDimension);
        }
    }
}
