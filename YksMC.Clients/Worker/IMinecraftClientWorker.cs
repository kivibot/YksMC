using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Connection;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Clients.Worker
{
    public interface IMinecraftClientWorker
    {
        void StartHandling(IMinecraftConnection connection);
        void EnqueuePacket(IPacket packet);
        void SetState(ConnectionState state);
    }
}
