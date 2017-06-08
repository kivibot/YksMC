using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Models;

namespace YksMC.Abstraction.Services
{
    public interface IEntityService
    {
        IPlayer CreatePlayer(int entityId, Dimension dimension, int gamemode);
        IPlayer GetPlayer();

        void UpdatePlayerPosition(Vector3<double> position);
        void UpdatePlayerLookDirection(LookDirection lookDirection);
    }
}
