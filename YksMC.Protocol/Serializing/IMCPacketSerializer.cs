using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Protocol.Serializing
{
    public interface IMCPacketSerializer
    {
        void Serialize(object value, IMCPacketWriter writer);
    }
}
