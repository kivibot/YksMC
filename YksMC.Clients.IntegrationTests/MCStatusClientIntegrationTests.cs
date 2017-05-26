using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Protocol.IntegrationTests.Config;
using YksMC.Clients;
using YksMC.Clients.Models.Dtos;
using YksMC.Protocol;

namespace YksMC.Clients.IntegrationTests
{
    [TestFixture]
    public class MCStatusClientIntegrationTests
    {
        [Test, MaxTime(10000)]
        public async Task TestFull()
        {
            ServerConfig config = ConfigManager.GetServerConfig();

            MCStatusClientBuilder builder = new MCStatusClientBuilder();
            builder.UsingServer(config.Host, config.Port);
            IMCStatusClient client = await builder.BuildAsync();

            StatusDto status = await client.GetStatusAsync();
            TimeSpan ping = await client.GetPingAsync();

            Assert.AreEqual(0, status.Players.Online);
            Assert.AreNotEqual(0, ping.Ticks);
        }
    }
}
