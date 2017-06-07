using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Models;

namespace YksMC.Abstraction.Services
{
    public interface IEntityService
    {
        IEntity CreateEntity(int entityId);

        void SetLocation(int entityId, Location location);
        void SetRotation(int entityId, Rotation rotation);
        void SetVelocity(int entityId, Vector3<double> velocity);
    }
}
