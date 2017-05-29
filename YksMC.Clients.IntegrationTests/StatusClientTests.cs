using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using YksMC.Clients.Models.Dtos;

namespace YksMC.Clients.IntegrationTests
{
    [TestFixture]
    public class StatusClientTests
    {

        [Test]
        public async Task MCStatusClient_WithRealServer_ReturnsValidValues()
        {
            using (MCStatusClient client = new MCStatusClient())
            {
                await client.ConnectAsync("localhost", 25565);
                ServerStatus status = await client.GetStatusAsync();

                Assert.AreEqual(20, status.Players.Max);
                Assert.AreNotEqual(0, status.Ping.Ticks);
            }            
        }


    }
}
