using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.MCProtocol.Models.Packets;

namespace YksMC.MCProtocol
{
    public interface IMCPacketSerializer
    {
        void Serialize<T>(T value, IMCPacketWriter writer) where T : AbstractPacket;
    }
}
