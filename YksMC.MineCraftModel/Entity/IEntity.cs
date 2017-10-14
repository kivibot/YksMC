using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.EntityType;

namespace YksMC.MinecraftModel.Entity
{
    public interface IEntity
    {
        int Id { get; }
        IEntityType Type { get; }
        IEntityCoordinate Position { get; }
        
        double Yaw { get; }
        double Pitch { get; }
        double HeadYaw { get; }

        IEntity ChangePosition(IEntityCoordinate position);
        IEntity ChangeLook(double yaw, double pitch);
        IEntity ChangeHeadLook(double headYaw);
    }
}
