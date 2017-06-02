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
    public class PacketSerializerTests
    {

        [Test]
        public void TestSerializeHandshakePacket()
        {
            PacketSerializer serializer = new PacketSerializer();
            HandshakePacket packet = new HandshakePacket()
            {
                Id = new VarInt(0x00),
                ProtocolVersion = ProtocolVersion.Unknown,
                ServerAddress = "ABC",
                ServerPort = 1234,
                NextState = ConnectionState.Status
            };
            FakePacketBuilder builder = new FakePacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { new VarInt(0x00), new VarInt(-1), "ABC", (ushort)1234, new VarInt(0x01) }, builder.Objects);
        }

        [Test]
        public void TestSerializeSupportsAllTypes()
        {
            PacketSerializer serializer = new PacketSerializer();
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
            FakePacketBuilder builder = new FakePacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { new VarInt(16), packet.Bool, packet.SignedByte, packet.Byte, packet.Short, packet.UnsignedShort,
                packet.Int, packet.UnsignedInt, packet.Long, packet.UnsignedLong, packet.Float, packet.Double, packet.String, packet.Chat,
                packet.VarInt, packet.VarLong, packet.Position, packet.Angle, packet.Guid }, builder.Objects);
        }

        [Test]
        public void TestSerializeDoesNotAcceptUnsupportedProperties()
        {
            PacketSerializer serializer = new PacketSerializer();
            InvalidPacket packet = new InvalidPacket();
            FakePacketBuilder builder = new FakePacketBuilder();

            Assert.Throws<ArgumentException>(() =>
            {
                serializer.Serialize(packet, builder);
            });
        }

        [Test]
        public void Serialize_VarIntEnum_Works()
        {
            PacketSerializer serializer = new PacketSerializer();
            GenericPacket<ProtocolVersion> packet = new GenericPacket<ProtocolVersion>() { Value = ProtocolVersion.Unknown };
            FakePacketBuilder builder = new FakePacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { new VarInt((int)ProtocolVersion.Unknown) }, builder.Objects);
        }

        [Test]
        public void Serialize_NullValue_Throws()
        {
            PacketSerializer serializer = new PacketSerializer();
            FakePacketBuilder builder = new FakePacketBuilder();

            Assert.Throws<ArgumentNullException>(() =>
            {
                serializer.Serialize(null, builder);
            });
        }

        [Test]
        public void Serialize_NullProperty_Throws()
        {
            PacketSerializer serializer = new PacketSerializer();
            GenericPacket<string> packet = new GenericPacket<string>() { Value = null };
            FakePacketBuilder builder = new FakePacketBuilder();

            Assert.Throws<ArgumentException>(() =>
            {
                serializer.Serialize(packet, builder);
            });
        }

        [Test]
        public void Serialize_ByteArray_Works()
        {
            PacketSerializer serializer = new PacketSerializer();
            GenericPacket<ByteArray> packet = new GenericPacket<ByteArray>() { Value = new ByteArray() { Length = new VarInt(4), Data = new byte[4] } };
            FakePacketBuilder builder = new FakePacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { new ByteArray() { Length = new VarInt(4), Data = new byte[4] } }, builder.Objects);
        }
    }
}