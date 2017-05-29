﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Clients.Models.Dtos;

namespace YksMC.Clients
{
    public interface IMCStatusClient
    {
        Task ConnectAsync(string host, ushort port, CancellationToken cancelToken = default(CancellationToken));
        Task<ServerStatus> GetStatusAsync(CancellationToken cancelToken = default(CancellationToken));
    }
}
