using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Clients.Models.Status;

namespace YksMC.Clients
{
    public interface IMCStatusClient
    {
        Task<ServerStatus> GetStatusAsync(string host, ushort port, CancellationToken cancelToken = default(CancellationToken));
    }
}
