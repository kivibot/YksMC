﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.MCProtocol;
using YksMC.MCProtocol.Models.Types;

namespace YksMc.MCProtocol.Tests.Fakes
{
    public class FakeMCPacketSink : IMCPacketSink
    {
        private readonly List<byte[]> _packets = new List<byte[]>();

        public IReadOnlyList<byte[]> Packets => _packets;

        public Task SendPacketAsync(byte[] data, CancellationToken cancelToken = default(CancellationToken))
        {
            _packets.Add(data);
            return Task.FromResult(true);
        }
    }
}
