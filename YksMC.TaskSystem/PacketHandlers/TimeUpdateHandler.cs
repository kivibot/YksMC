using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Clientbound;

namespace YksMC.Behavior.PacketHandlers
{
    public class TimeUpdateHandler : WorldEventHandler, IWorldEventHandler<TimeUpdatePacket>
    {
        public IWorldEventResult Handle(IWorldEvent<TimeUpdatePacket> message)
        {
            IWorld world = message.World;
            TimeUpdatePacket packet = message.Event;
            IDimension dimension = world.GetCurrentDimension();
            if (dimension == null)
            {
                throw new ArgumentException("Time update before dimension initialization!");
            }
            return Result(world.ReplaceCurrentDimension(
                dimension.ChangeAgeAndTime(
                    ageAndTime => ageAndTime.ChangeAgeAndTime(packet.WorldAge, packet.TimeOfDay)
                )
            ));
        }
    }
}
