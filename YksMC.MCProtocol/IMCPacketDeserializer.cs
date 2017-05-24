﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.MCProtocol.Models.Packets;

namespace YksMC.MCProtocol
{
    public interface IMCPacketDeserializer
    {
        T Deserialize<T>(IMCPacketReader reader) where T : AbstractPacket;
    }
}
