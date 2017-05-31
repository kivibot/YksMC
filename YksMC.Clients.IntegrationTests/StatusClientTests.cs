using Autofac;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using YksMC.Clients.Models.Dtos;
using YksMC.Protocol;
using YksMC.Protocol.Serializing;

namespace YksMC.Clients.IntegrationTests
{
    [TestFixture]
    public class StatusClientTests
    {

        private IContainer _container;

        [SetUp]
        public void SetUp()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<MCStatusClient>();
            builder.RegisterType<MCPacketClient>().AsImplementedInterfaces();
            builder.RegisterType<TcpClient>();
            builder.RegisterType<MCPacketSerializer>().AsImplementedInterfaces();
            builder.RegisterType<MCPacketDeserializer>().AsImplementedInterfaces();
            builder.RegisterType<MCPacketReader>().AsImplementedInterfaces();
            builder.RegisterType<MCPacketBuilder>().AsImplementedInterfaces();
            builder.RegisterType<StreamMCConnection>().AsSelf();

            _container = builder.Build();
        }

        [TearDown]
        public void TearDown()
        {
            _container.Dispose();
        }


        [Test]
        public async Task MCStatusClient_WithRealServer_ReturnsValidValues()
        {
            MCStatusClient client = _container.Resolve<MCStatusClient>();

            ServerStatus status = await client.GetStatusAsync("localhost", 25565);

            Assert.AreEqual(20, status.Players.Max);
            Assert.AreNotEqual(0, status.Ping.Ticks);
        }


    }
}
