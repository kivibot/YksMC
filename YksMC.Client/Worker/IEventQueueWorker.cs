using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Client.Worker
{
    public interface IEventQueueWorker
    {
        void StartHandling();
        void EnqueueEvent(IPacket packet);
    }
}
