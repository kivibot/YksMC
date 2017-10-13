using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Bot.Handlers
{
    public class TimeUpdateHandler : IWorldEventHandler<TimeUpdatePacket>
    {
        public IWorld ApplyEvent(TimeUpdatePacket packet, IWorld world)
        {
            IDimension dimension = world.GetCurrentDimension();
            if (dimension == null)
            {
                throw new ArgumentException("Time update before dimension initialization!");
            }
            return world.ReplaceCurrentDimension(
                dimension.ChangeAgeAndTime(
                    ageAndTime => ageAndTime.ChangeAgeAndTime(packet.WorldAge, packet.TimeOfDay)
                )
            );
        }
    }
}
