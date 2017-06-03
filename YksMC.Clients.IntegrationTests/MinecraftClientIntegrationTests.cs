﻿using Autofac;
using NUnit.Framework;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using YksMC.Clients.Handlers;
using YksMC.Clients.Injection;
using YksMC.Clients.Mapper;
using YksMC.Clients.Models.Status;
using YksMC.Clients.Worker;
using YksMC.Protocol;
using YksMC.Protocol.Connection;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Packets;
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
            builder.RegisterType<TcpClient>();
            builder.RegisterType<PacketSerializer>().AsImplementedInterfaces();
            builder.RegisterType<PacketDeserializer>().AsImplementedInterfaces();
            builder.RegisterType<PacketReader>().AsImplementedInterfaces();
            builder.RegisterType<PacketBuilder>().AsImplementedInterfaces();
            builder.RegisterType<StreamMinecraftConnection>().AsSelf();
            builder.RegisterType<MinecraftClientWorker>().AsImplementedInterfaces();
            builder.RegisterType<AutofacPacketHandlerFactory>().AsImplementedInterfaces();
            builder.RegisterType<MinecraftClientWorkerOptions>();
            builder.RegisterInstance(Log.Logger);
            PacketTypeMapper typeMapper = new PacketTypeMapper();
            typeMapper.RegisterVanillaPackets();
            builder.RegisterInstance(typeMapper).AsImplementedInterfaces();

            builder.RegisterType<LoginHandler>().AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(AutofacOwnedWrapper<>)).AsImplementedInterfaces();

            _container = builder.Build();
        }

        [TearDown]
        public void TearDown()
        {
            _container.Dispose();
        }

        [Test]
        public async Task ConnectAsync_WithRealServer_DoesNotCrash()
        {
            MinecraftClient client = _container.Resolve<MinecraftClient>();

            await client.ConnectAsync("localhost", 25565);
            client.SendPacket(new HandshakePacket() { NextState = Protocol.Models.Constants.ConnectionState.Login, ProtocolVersion = Protocol.Models.Constants.ProtocolVersion.Unknown, ServerAddress = "", ServerPort = 56 });
            client.SetState(ConnectionState.Login);

            await Task.Delay(100000);
        }

    }
}
