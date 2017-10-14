using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Client
{
    public interface IMinecraftClientPacketHandler
    {
        void HandlePacket(object packet);
    }
}
