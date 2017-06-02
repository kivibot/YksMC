using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Protocol.Serializing
{
    public interface IPacketDeserializer
    {
        T Deserialize<T>(IPacketReader reader);
        object Deserialize(IPacketReader reader, Type type);
    }
}
