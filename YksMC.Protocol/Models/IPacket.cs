﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Models
{
    public interface IPacket 
    {
        VarInt PacketId { get; set; }
    }
}