using Autofac;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using YksMC.Clients.Mapper;
using YksMC.Clients.Models.Status;
using YksMC.Protocol;
using YksMC.Protocol.Connection;
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
            builder.RegisterType<PacketSerializer>().AsImplementedInterfaces();
            builder.RegisterType<PacketDeserializer>().AsImplementedInterfaces();
            builder.RegisterType<PacketReader>().AsImplementedInterfaces();
            builder.RegisterType<PacketBuilder>().AsImplementedInterfaces();
            builder.RegisterType<StreamMinecraftConnection>().AsSelf();
            PacketTypeMapper typeMapper = new PacketTypeMapper();
            typeMapper.RegisterVanillaPackets();
            builder.RegisterInstance(typeMapper).AsImplementedInterfaces();

            _container = builder.Build();
        }

        [TearDown]
        public void TearDown()
        {
            _container.Dispose();
        }
    }
}
