using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Clients
{
    public interface IMCClientBuilder
    {
        IMCClientBuilder UsingServer(string host, ushort port);
    }
}
