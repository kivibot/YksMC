﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Protocol.Tests.Fakes;
using YksMC.Protocol.Tests.Models;
using YksMC.Protocol;
using YksMC.Protocol.Models.Packets;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Serializing;
using YksMC.Protocol.Models.Constants;

namespace YksMC.Protocol.Tests.Serializing
{
    [TestFixture]
    public class MCPacketSerializerTests
    {

        [Test]
        public void TestSerializeHandshakePacket()
        {
            MCPacketSerializer serializer = new MCPacketSerializer();
            HandshakePacket packet = new HandshakePacket()
            {
                Id = new VarInt(0x00),
                ProtocolVersion = ProtocolVersion.Unknown,
                ServerAddress = "ABC",
                ServerPort = 1234,
                NextState = ConnectionState.Status
            };
            FakeMCPacketBuilder builder = new FakeMCPacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { new VarInt(0x00), new VarInt(-1), "ABC", (ushort)1234, new VarInt(0x01) }, builder.Objects);
        }

        [Test]
        public void TestSerializeSupportsAllTypes()
        {
            MCPacketSerializer serializer = new MCPacketSerializer();
            TestPacket packet = new TestPacket()
            {
                Id = new VarInt(16),
                Bool = true,
                SignedByte = -3,
                Byte = 200,
                Short = -6000,
                UnsignedShort = 60000,
                Int = -1000000,
                UnsignedInt = 1000000,
                Long = long.MinValue,
                UnsignedLong = ulong.MaxValue,
                Float = 12.3f,
                Double = 133.7,
                String = "Testi",
                Chat = new Chat("Chatti"),
                VarInt = new VarInt(-1),
                VarLong = new VarLong(-1),
                Position = new Position(1, 2, 3),
                Angle = new Angle(127),
                Guid = Guid.NewGuid()
            };
            FakeMCPacketBuilder builder = new FakeMCPacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { new VarInt(16), packet.Bool, packet.SignedByte, packet.Byte, packet.Short, packet.UnsignedShort,
                packet.Int, packet.UnsignedInt, packet.Long, packet.UnsignedLong, packet.Float, packet.Double, packet.String, packet.Chat,
                packet.VarInt, packet.VarLong, packet.Position, packet.Angle, packet.Guid }, builder.Objects);
        }

        [Test]
        public void TestSerializeDoesNotAcceptUnsupportedProperties()
        {
            MCPacketSerializer serializer = new MCPacketSerializer();
            InvalidPacket packet = new InvalidPacket();
            FakeMCPacketBuilder builder = new FakeMCPacketBuilder();

            Assert.Throws<ArgumentException>(() =>
            {
                serializer.Serialize(packet, builder);
            });
        }

        [Test]
        public void Serialize_VarIntEnum_Works()
        {
            MCPacketSerializer serializer = new MCPacketSerializer();
            GenericPacket<ProtocolVersion> packet = new GenericPacket<ProtocolVersion>() { Value = ProtocolVersion.Unknown };
            FakeMCPacketBuilder builder = new FakeMCPacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { new VarInt((int)ProtocolVersion.Unknown) }, builder.Objects);
        }

        [Test]
        public void Serialize_NullValue_Throws()
        {
            MCPacketSerializer serializer = new MCPacketSerializer();
            FakeMCPacketBuilder builder = new FakeMCPacketBuilder();

            Assert.Throws<ArgumentNullException>(() =>
            {
                serializer.Serialize(null, builder);
            });
        }

        [Test]
        public void Serialize_NullProperty_Throws()
        {
            MCPacketSerializer serializer = new MCPacketSerializer();
            GenericPacket<string> packet = new GenericPacket<string>() { Value = null };
            FakeMCPacketBuilder builder = new FakeMCPacketBuilder();

            Assert.Throws<ArgumentException>(() =>
            {
                serializer.Serialize(packet, builder);
            });
        }

        [Test]
        public void Serialize_ByteArray_Works()
        {
            MCPacketSerializer serializer = new MCPacketSerializer();
            GenericPacket<ByteArray> packet = new GenericPacket<ByteArray>() { Value = new ByteArray() { Length = new VarInt(4), Data = new byte[4] } };
            FakeMCPacketBuilder builder = new FakeMCPacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { new ByteArray() { Length = new VarInt(4), Data = new byte[4] } }, builder.Objects);
        }
    }
}
