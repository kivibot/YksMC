using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.MCProtocol.Models.Packets;

namespace YksMC.MCProtocol
{
    public interface IMCPacketDeserializer
    {
        AbstractPacket Deserialize(IMCPacketReader reader);

        void RegisterType<T>(int id) where T : AbstractPacket;
    }
}
