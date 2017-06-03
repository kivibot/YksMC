using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Connection;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Constants;

namespace YksMC.Client.Worker
{
    public interface IMinecraftClientWorker
    {
        void StartHandling(IMinecraftConnection connection);
        void EnqueuePacket(IPacket packet);
        void SetState(ConnectionState state);
    }
}
