using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YksMC.Protocol.Serializing
{
    public interface IPacketSerializer
    {
        void Serialize(object value, IPacketBuilder builder);
    }
}
