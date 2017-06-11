﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Client;
using YksMC.Client.EventBus;
using YksMC.Protocol.Packets.Play.Common;

namespace YksMC.Bot.Handlers
{
    public class KeepAliveHandler : IEventHandler<KeepAlivePacket>
    {
        private IMinecraftClient _client;

        public KeepAliveHandler(IMinecraftClient client)
        {
            _client = client;
        }

        public void Handle(KeepAlivePacket packet)
        {
            _client.SendKeepAlive(packet.KeepAliveId);
        }
    }
}
