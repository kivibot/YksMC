using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Common;
using YksMC.MinecraftModel.EntityType;

namespace YksMC.MinecraftModel.Entity
{
    public class Entity : IEntity
    {
        private readonly int _id;
        private readonly IEntityType _type;
        private readonly IEntityLocation _location;
        private readonly double _yaw;
        private readonly double _pitch;
        private readonly double _headYaw;
        private readonly bool _isOnGround;
        private readonly IVector3<double> _velocity;
        private readonly int _health;

        public int Id => _id;
        public IEntityType Type => _type;
        public IEntityLocation Location => _location;
        public double Yaw => _yaw;
        public double Pitch => _pitch;
        public double HeadYaw => _headYaw;
        public bool IsOnGround => _isOnGround;
        public IVector3<double> Velocity => _velocity;

        public int Health => _health;
        public bool IsAlive => _health > 0;

        public Entity(int id, IEntityType type, IEntityLocation location, double yaw, double pitch, double headYaw, bool isOnGround, IVector3<double> velocity, int health)
        {
            _id = id;
            _type = type;
            _location = location;
            _yaw = yaw;
            _pitch = pitch;
            _headYaw = headYaw;
            _isOnGround = isOnGround;
            _velocity = velocity;
            _health = health;
        }

        public IEntity ChangeLocation(IEntityLocation location)
        {
            return new Entity(_id, _type, location, _yaw, _pitch, _headYaw, _isOnGround, _velocity, _health);
        }

        public IEntity ChangeLook(double yaw, double pitch)
        {
            return new Entity(_id, _type, _location, yaw, pitch, _headYaw, _isOnGround, _velocity, _health);
        }

        public IEntity ChangeHeadLook(double headYaw)
        {
            return new Entity(_id, _type, _location, _yaw, _pitch, headYaw, _isOnGround, _velocity, _health);
        }

        public IEntity ChangeOnGround(bool isOnGround)
        {
            return new Entity(_id, _type, _location, _yaw, _pitch, _headYaw, isOnGround, _velocity, _health);
        }

        public IEntity ChangeVelocity(IVector3<double> velocity)
        {
            return new Entity(_id, _type, _location, _yaw, _pitch, _headYaw, _isOnGround, velocity, _health);
        }

        public IEntity ChangeHealth(int health)
        {
            return new Entity(_id, _type, _location, _yaw, _pitch, _headYaw, _isOnGround, _velocity, health);
        }
    }
}
