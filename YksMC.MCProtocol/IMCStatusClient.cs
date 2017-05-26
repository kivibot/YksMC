using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.MCProtocol.Models.Dtos;

namespace YksMC.MCProtocol
{
    public interface IMCStatusClient : IMCClient
    {   
        Task<StatusDto> GetStatusAsync(CancellationToken cancelToken = default(CancellationToken));
        Task<TimeSpan> GetPingAsync(CancellationToken cancelToken = default(CancellationToken));
    }
}
