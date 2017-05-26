using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MCProtocol
{
    public interface IMCClientBuilder
    {
        IMCClientBuilder UsingServer(string host, ushort port);
    }
}
