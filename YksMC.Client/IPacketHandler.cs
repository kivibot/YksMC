using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YksMC.Client
{
    public interface IPacketHandler<T>
    {
        Task HandleAsync(T packet);
    }
}
