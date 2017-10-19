using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.Window;

namespace YksMC.MinecraftModel.World
{
    public interface IWorld
    {
        IWindowCollection Windows { get; }

        IWorld ReplacePlayer(IPlayer player);
        IWorld ReplaceLocalPlayer(IPlayer player);
        IEnumerable<IPlayer> GetPlayers();
        IPlayer GetLocalPlayer();

        IWorld ReplaceDimension(IDimension dimension);
        IWorld ReplaceCurrentDimension(IDimension dimension);
        IDimension GetCurrentDimension();
        IDimension GetDimension(int id);

        IWorld ReplaceWindow(IWindow window);

    }
}
