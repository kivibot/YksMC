using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.World;

namespace YksMC.Abstraction.Misc
{
    public interface IEntityService
    {
        IPlayer CreatePlayer(int entityId, Guid uuid, Dimension dimension);

        void SetPlayerPosition(int entityId, Vector3<double> position);
        void SetPlayerLookDirection(int entityId, LookDirection lookDirection);
    }
}
