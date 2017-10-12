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
        //TODO: use better constant
        private const long _ticksPerDay = 24000;

        public IWorld ApplyEvent(TimeUpdatePacket packet, IWorld world)
        {
            IDimension dimension = world.GetCurrentDimension();
            if (dimension == null)
            {
                throw new ArgumentException("Time update before dimension initialization!");
            }
            return world.ReplaceCurrentDimension(dimension.ChangeAgeAndTime(packet.WorldAge, packet.TimeOfDay % _ticksPerDay));
        }
    }
}
