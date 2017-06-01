using Autofac;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using YksMC.Clients.Models.Status;
using YksMC.Protocol;
using YksMC.Protocol.Serializing;

namespace YksMC.Clients.IntegrationTests
{
    [TestFixture]
    public class MinecraftClientIntegrationTests
    {

        private IContainer _container;

        [SetUp]
        public void SetUp()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<MinecraftClient>();
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
        public async Task GetStatusAsync_WithRealServer_ReturnsValidValues()
        {
            MinecraftClient client = _container.Resolve<MinecraftClient>();

            await client.ConnectAsync("localhost", 25565);
            ServerStatus status = await client.GetStatusAsync();

            Assert.AreEqual(20, status.Players.Max);
            Assert.AreNotEqual(0, status.Ping.Ticks);
        }

        [Test]
        public async Task LoginAsync_WithRealServer_DoesNotCrash()
        {
            MinecraftClient client = _container.Resolve<MinecraftClient>();

            await client.ConnectAsync("localhost", 25565);
            await client.LoginAsync();
        }

    }
}
