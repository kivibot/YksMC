using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Clients.Models.Dtos;

namespace YksMC.Clients
{
    public interface IMCStatusClient : IMCClient
    {   
        Task<StatusDto> GetStatusAsync(CancellationToken cancelToken = default(CancellationToken));
        Task<TimeSpan> GetPingAsync(CancellationToken cancelToken = default(CancellationToken));
    }
}
