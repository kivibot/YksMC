using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.EntityType;

namespace YksMC.MinecraftModel.Entity
{
    public class Entity : IEntity
    {
        private readonly int _id;
        private readonly IEntityType _type;
        private readonly IEntityCoordinate _position;
        private readonly double _yaw;
        private readonly double _pitch;
        private readonly double _headYaw;

        public int Id => _id;
        public IEntityType Type => _type;
        public IEntityCoordinate Position => _position;

        public double Yaw => _yaw;
        public double Pitch => _pitch;
        public double HeadYaw => _headYaw;

        public Entity(int id, IEntityType type, IEntityCoordinate position, double yaw, double pitch, double headYaw)
        {
            _id = id;
            _type = type;
            _position = position;
        }

        public IEntity ChangePosition(IEntityCoordinate position)
        {
            return new Entity(_id, _type, position, _yaw, _pitch, _headYaw);
        }

        public IEntity ChangeLook(double yaw, double pitch)
        {
            return new Entity(_id, _type, _position, yaw, pitch, _headYaw);
        }

        public IEntity ChangeHeadLook(double headYaw)
        {
            return new Entity(_id, _type, _position, _yaw, _pitch, headYaw);
        }
    }
}
