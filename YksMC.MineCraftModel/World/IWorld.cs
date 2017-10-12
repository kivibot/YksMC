using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Player;

namespace YksMC.MinecraftModel.World
{
    public interface IWorld
    {
        IWorld ReplacePlayer(IPlayer player);
        IWorld ReplaceLocalPlayer(IPlayer player);
        IEnumerable<IPlayer> GetPlayers();
        IPlayer GetLocalPlayer();

        IWorld ReplaceDimension(IDimension dimension);
        IWorld ReplaceCurrentDimension(IDimension dimension);
        IDimension GetCurrentDimension();
        IDimension GetDimension(int id);
    }
}
