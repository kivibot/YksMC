﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YksMC.Clients
{
    public interface IMCStatusClientBuilder : IMCClientBuilder
    {
        Task<IMCStatusClient> BuildAsync(CancellationToken cancelToken = default(CancellationToken));
    }
}