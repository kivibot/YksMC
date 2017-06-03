using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Client.Models
{
    public class ServerAddress
    {
        public string Host { get; set; }
        public ushort Port { get; set; }

        public ServerAddress(string host, ushort port)
        {
            Host = host;
            Port = port;
        }
    }
}
