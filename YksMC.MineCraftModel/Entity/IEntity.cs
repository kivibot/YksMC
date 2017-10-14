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
        IEntityLocation Location { get; }
        
        double Yaw { get; }
        double Pitch { get; }
        double HeadYaw { get; }

        IEntity ChangeLocation(IEntityLocation location);
        IEntity ChangeLook(double yaw, double pitch);
        IEntity ChangeHeadLook(double headYaw);
    }
}
