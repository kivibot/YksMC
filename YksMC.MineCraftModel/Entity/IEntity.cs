using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Common;
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

        IVector3<double> Velocity { get; }

        bool IsOnGround { get; }

        IEntity ChangeLocation(IEntityLocation location);
        IEntity ChangeLook(double yaw, double pitch);
        IEntity ChangeHeadLook(double headYaw);
        IEntity ChangeOnGround(bool isOnGround);
        IEntity ChangeVelocity(IVector3<double> velocity);
    }
}
