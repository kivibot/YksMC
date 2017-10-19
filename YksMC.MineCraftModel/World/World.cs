using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.Window;

namespace YksMC.MinecraftModel.World
{
    public class World : IWorld
    {
        private readonly IReadOnlyDictionary<Guid, IPlayer> _players;
        private readonly IPlayer _localPlayer;
        private readonly IReadOnlyDictionary<int, IDimension> _dimensions;
        private readonly IDimension _currentDimension;
        private readonly IWindowCollection _windows;

        public World(IReadOnlyDictionary<Guid, IPlayer> players, IPlayer localPlayer, IReadOnlyDictionary<int, IDimension> dimensions, IDimension currentDimension, IWindowCollection windows)
        {
            _players = players;
            _localPlayer = localPlayer;
            _dimensions = dimensions;
            _currentDimension = currentDimension;
            _windows = windows;
        }

        public IWindowCollection Windows => _windows;

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
            return new World(_players, _localPlayer, dimensions, dimension, _windows);
        }

        public IWorld ReplaceDimension(IDimension dimension)
        {
            if (dimension == null)
            {
                throw new ArgumentNullException(nameof(dimension));
            }
            if (_currentDimension?.Id == dimension.Id)
            {
                return ReplaceCurrentDimension(dimension);
            }

            Dictionary<int, IDimension> dimensions = _dimensions.ToDictionary(e => e.Key, e => e.Value);
            dimensions[dimension.Id] = dimension;
            return new World(_players, _localPlayer, dimensions, _currentDimension, _windows);
        }

        public IWorld ReplaceLocalPlayer(IPlayer player)
        {
            Dictionary<Guid, IPlayer> players = _players.ToDictionary(e => e.Key, e => e.Value);
            players[player.Id] = player;
            return new World(players, player, _dimensions, _currentDimension, _windows);
        }

        public IWorld ReplacePlayer(IPlayer player)
        {
            if (_localPlayer?.Id == player.Id)
            {
                return ReplaceLocalPlayer(player);
            }

            Dictionary<Guid, IPlayer> players = _players.ToDictionary(e => e.Key, e => e.Value);
            players[player.Id] = player;
            return new World(players, _localPlayer, _dimensions, _currentDimension, _windows);
        }

        public IWorld ReplaceWindow(IWindow window)
        {
            IWindowCollection windows = _windows.ReplaceWindow(window);
            return new World(_players, _localPlayer, _dimensions, _currentDimension, windows);
        }
    }
}
