using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Packets.Play.Common;

namespace YksMC.Behavior.PacketHandlers
{
    public class KeepAliveHandler : WorldEventHandler, IWorldEventHandler<KeepAlivePacket>
    {
        public IWorldEventResult Handle(IWorldEvent<KeepAlivePacket> message)
        {
            KeepAlivePacket reply = new KeepAlivePacket()
            {
                KeepAliveId = message.Event.KeepAliveId
            };
            return Result(message.World, reply);
        }
    }
}
