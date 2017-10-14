using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Common;

namespace YksMC.Bot.Handlers
{
    public class KeepAliveHandler : WorldEventHandler, IWorldEventHandler<KeepAlivePacket>
    {
        public IWorldEventResult ApplyEvent(KeepAlivePacket packet, IWorld world)
        {
            KeepAlivePacket reply = new KeepAlivePacket()
            {
                KeepAliveId = packet.KeepAliveId
            };
            return Result(world, reply);
        }
    }
}
