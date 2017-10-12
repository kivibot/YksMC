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
        private readonly IReadOnlyDictionary<IPlayerId, IPlayer> _players;
        private readonly IReadOnlyDictionary<int, IDimension> _dimensions;
        private readonly IDimension _currentDimension;

        public World(IReadOnlyDictionary<IPlayerId, IPlayer> players, IReadOnlyDictionary<int, IDimension> dimensions, IDimension currentDimension)
        {
            _players = players;
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
            //TODO: fix
            return _players.Values.First();
        }

        public IEnumerable<IPlayer> GetPlayers()
        {
            return _players.Values;
        }

        public IWorld ReplaceCurrentDimension(IDimension dimension)
        {
            Dictionary<int, IDimension> dimensions = _dimensions.ToDictionary(e => e.Key, e => e.Value);
            dimensions[dimension.Id] = dimension;
            return new World(_players, dimensions, dimension);
        }

        public IWorld ReplacePlayer(IPlayer player)
        {
            Dictionary<IPlayerId, IPlayer> players = _players.ToDictionary(e => e.Key, e => e.Value);
            players[player.Id] = player;
            return new World(players, _dimensions, _currentDimension);
        }
    }
}
