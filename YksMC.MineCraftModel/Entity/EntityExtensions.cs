using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Common;

namespace YksMC.MinecraftModel.Entity
{
    public static class EntityExtensions
    {

        public static IEntity LookAt(this IEntity entity, IEntityLocation target)
        {
            IVector3<double> lookVector = target.AsVector().Substract(entity.Location.AsVector());
            if (lookVector.Length() == 0)
            {
                return entity;
            }

            double pitch = -Math.Atan2(lookVector.Y, new Vector3d(lookVector.X, 0, lookVector.Z).Length());
            double yaw = -Math.Atan2(lookVector.X, lookVector.Z);

            return entity.ChangeLook(yaw, pitch);
        }

    }
}
