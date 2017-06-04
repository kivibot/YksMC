using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Protocol.Tests.Fakes;
using YksMC.Protocol.Tests.Models;
using YksMC.Protocol;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Serializing;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Packets;

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
                ProtocolVersion = ProtocolVersion.Unknown,
                ServerAddress = "ABC",
                ServerPort = 1234,
                NextState = ConnectionState.Status
            };
            FakePacketBuilder builder = new FakePacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { new VarInt(-1), "ABC", (ushort)1234, new VarInt(0x01) }, builder.Objects);
        }

        [Test]
        public void TestSerializeSupportsAllTypes()
        {
            PacketSerializer serializer = new PacketSerializer();
            TestPacket packet = new TestPacket()
            {
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

            Assert.AreEqual(new object[] { packet.Bool, packet.SignedByte, packet.Byte, packet.Short, packet.UnsignedShort,
                packet.Int, packet.UnsignedInt, packet.Long, packet.UnsignedLong, packet.Float, packet.Double, packet.String, packet.Chat,
                packet.VarInt, packet.VarLong, packet.Position, packet.Angle, packet.Guid }, builder.Objects);
        }

        [Test]
        public void TestSerializeDoesNotAcceptUnsupportedProperties()
        {
            PacketSerializer serializer = new PacketSerializer();
            GenericPacket<DateTime> packet = new GenericPacket<DateTime>();
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

            Assert.Throws<ArgumentNullException>(() =>
            {
                serializer.Serialize(packet, builder);
            });
        }

        [Test]
        public void Serialize_IntegerVarArray_Works()
        {
            PacketSerializer serializer = new PacketSerializer();
            GenericPacket<VarArray<int>> packet = new GenericPacket<VarArray<int>>() { Value = new VarArray<int>() { Count = new VarInt(4), Values = new int[4] } };
            FakePacketBuilder builder = new FakePacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { new VarInt(4), 0, 0, 0, 0 }, builder.Objects);
        }

        [Test]
        public void Serialize_ByteVarArray_UsesByteArray()
        {
            PacketSerializer serializer = new PacketSerializer();
            GenericPacket<VarArray<byte>> packet = new GenericPacket<VarArray<byte>>() { Value = new VarArray<byte>() { Count = new VarInt(4), Values = new byte[4] } };
            FakePacketBuilder builder = new FakePacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { new VarInt(4), new byte[4] }, builder.Objects);
        }

        [Test]
        public void Serialize_Object_Works()
        {
            PacketSerializer serializer = new PacketSerializer();
            GenericPacket<GenericPacket<long>> packet = new GenericPacket<GenericPacket<long>>() { Value = new GenericPacket<long>() { Value = 1337 } };
            FakePacketBuilder builder = new FakePacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { 1337L }, builder.Objects);
        }

        [Test]
        public void Serialize_OptionalWithoutValue_Works()
        {
            PacketSerializer serializer = new PacketSerializer();
            Optional<string> value = new Optional<string>();
            FakePacketBuilder builder = new FakePacketBuilder();

            serializer.Serialize(value, builder);

            Assert.AreEqual(new object[] { false }, builder.Objects);
        }

        [Test]
        public void Serialize_OptionalWithValue_Works()
        {
            PacketSerializer serializer = new PacketSerializer();
            Optional<string> value = new Optional<string>()
            {
                HasValue = true,
                Value = "Test"
            };
            FakePacketBuilder builder = new FakePacketBuilder();

            serializer.Serialize(value, builder);

            Assert.AreEqual(new object[] { true, "Test" }, builder.Objects);
        }

        [Test]
        public void Serialize_ValidValueIsConditional_PutsIntValue()
        {
            PacketSerializer serializer = new PacketSerializer();
            ConditionalPacket packet = new ConditionalPacket()
            {
                Action = 2,
                Filler = "Filler",
                IntValue = 5
            };
            FakePacketBuilder builder = new FakePacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { 2, "Filler", 5 }, builder.Objects);
        }

        [Test]
        public void Serialize_InvalidValueIsConditional_DoesNotPutIntValue()
        {
            PacketSerializer serializer = new PacketSerializer();
            ConditionalPacket packet = new ConditionalPacket()
            {
                Action = 1,
                Filler = "Filler",
                IntValue = 5,
                StringValue = ""
            };
            FakePacketBuilder builder = new FakePacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { 1, "Filler", "" }, builder.Objects);
        }

        [Test]
        public void Serialize_ValidValueIsNotConditional_PutsStringValue()
        {
            PacketSerializer serializer = new PacketSerializer();
            ConditionalPacket packet = new ConditionalPacket()
            {
                Action = 1,
                Filler = "Filler",
                StringValue = "Test"
            };
            FakePacketBuilder builder = new FakePacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { 1, "Filler", "Test" }, builder.Objects);
        }

        [Test]
        public void Serialize_InvalidValueIsNotConditional_DoesNotPutStringValue()
        {
            PacketSerializer serializer = new PacketSerializer();
            ConditionalPacket packet = new ConditionalPacket()
            {
                Action = 2,
                Filler = "Filler",
                StringValue = "Test"
            };
            FakePacketBuilder builder = new FakePacketBuilder();

            serializer.Serialize(packet, builder);

            Assert.AreEqual(new object[] { 2, "Filler", default(int) }, builder.Objects);
        }

    }
}
