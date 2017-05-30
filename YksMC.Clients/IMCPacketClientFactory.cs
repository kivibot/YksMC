using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Clients
{
    public interface IMCPacketClientFactory
    {
        IMCPacketClient Create();
        void Release(IMCPacketClient client);
    }
}
