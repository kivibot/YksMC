﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Protocol.Serializing
{
    public interface IMCPacketDeserializer
    {
        T Deserialize<T>(IMCPacketReader reader);
        object Deserialize(IMCPacketReader reader, Type type);
    }
}
