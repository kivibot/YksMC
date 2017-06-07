using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Abstraction.Models
{
    public interface IEntity
    {
        int Id { get; }
        Guid UniqueId { get; }
        IEntityType Type { get; }
        Location Location { get; }
        Rotation Rotation { get; }
        int Data { get; }
        Vector3<float> Velocity { get; }
        bool IsOnGround { get; }
        //TODO: equipment

    }
}
