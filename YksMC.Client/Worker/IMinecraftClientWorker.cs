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
        void Start(IMinecraftConnection connection);
        void Stop();
        void EnqueuePacket(object packet);
        void SetState(ConnectionState state);

        event Action<object> PacketReceived;
    }
}
