﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Login
{
    [Packet(0x02, ConnectionState.Login, BoundTo.Client)]
    public class LoginSuccessPacket
    {
        public string UserId { get; set; }
        public string Username { get; set; }
    }
}
