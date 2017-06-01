using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Models.Constants
{
    public enum ConnectionState
    {
        None = 0,
        Handshake = 200,
        Status = 1,
        Login = 2
    }
}
