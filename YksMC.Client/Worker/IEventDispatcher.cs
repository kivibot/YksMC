using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Protocol.Models;

namespace YksMC.Client.Worker
{
    public interface IEventDispatcher
    {
        Task DispatchEventAsync(object packet);
    }
}
