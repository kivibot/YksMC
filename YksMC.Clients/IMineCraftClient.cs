﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Clients.Models.Status;
using YksMC.Protocol.Models.Constants;

namespace YksMC.Clients
{
    public interface IMinecraftClient
    {
        ProtocolVersion ProtocolVersion { get; }

        Task ConnectAsync(string host, ushort port, CancellationToken cancelToken = default(CancellationToken));
        Task<ServerStatus> GetStatusAsync(CancellationToken cancelToken = default(CancellationToken));
        Task LoginAsync(CancellationToken cancelToken = default(CancellationToken));
    }
}
