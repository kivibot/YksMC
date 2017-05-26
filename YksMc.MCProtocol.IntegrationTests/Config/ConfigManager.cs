using System;
using System.Collections.Generic;
using System.Text;

namespace YksMc.MCProtocol.IntegrationTests.Config
{
    public static class ConfigManager
    {
        public static ServerConfig GetServerConfig()
        {
            return new ServerConfig()
            {
                Host = "localhost",
                Port = 25565
            };
        }
    }
}
