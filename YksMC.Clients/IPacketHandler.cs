using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YksMC.Clients
{
    public interface IPacketHandler<T>
    {
        Task HandleAsync<T>(T packet);
    }
}
