﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YksMc.Protocol.IntegrationTests.Config
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
