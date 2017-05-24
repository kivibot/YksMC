using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMc.MCProtocol.Tests.Fakes;
using YksMc.MCProtocol.Tests.Models;
using YksMc.MCProtocol.Tests.TestUtils;
using YksMC.MCProtocol;
using YksMC.MCProtocol.Models.Packets;
using YksMC.MCProtocol.Models.Types;

namespace YksMc.MCProtocol.Tests
{
    [TestFixture]
    public class MCPacketDeserializerTests
    {
        [Test]
        public void TestDeserializeHandshakePacket()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            deserializer.RegisterType<HandshakePacket>(HandshakePacket.PacketId);
            HandshakePacket expected = new HandshakePacket()
            {
                Id = new VarInt(0x00),
                ProtocolVersion = new VarInt(0x12),
                ServerAddress = "ABC",
                ServerPort = 1234,
                NextState = new VarInt(0x01)
            };
            FakeMCPacketReader reader = new FakeMCPacketReader(new VarInt(0x00), new VarInt(0x12), "ABC", (ushort)1234, new VarInt(0x01));

            AbstractPacket result = deserializer.Deserialize(reader);

            Assert.IsTrue(ComparisonUtil.Compare(expected, result));
        }

        [Test]
        public void TestDeserializeCanDeserializeAllTypes()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            deserializer.RegisterType<TestPacket>(123);
            TestPacket expected = new TestPacket()
            {
                Id = new VarInt(123),
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
            FakeMCPacketReader reader = new FakeMCPacketReader(expected.Id, expected.Bool, expected.SignedByte, expected.Byte, expected.Short, expected.UnsignedShort,
                expected.Int, expected.UnsignedInt, expected.Long, expected.UnsignedLong, expected.Float, expected.Double, expected.String, expected.Chat,
                expected.VarInt, expected.VarLong, expected.Position, expected.Angle, expected.Guid);

            AbstractPacket result = deserializer.Deserialize(reader);

            Assert.IsTrue(ComparisonUtil.Compare(expected, result));
        }

        [Test]
        public void TestRegisterTypeDoesNotAcceptUnsupportedProperties()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();

            Assert.Throws<ArgumentException>(() =>
            {
                deserializer.RegisterType<InvalidPacket>(0x00);
            });
        }

        [Test]
        public void TestDeserializeThrowsOnAnUnsupportedPacketId()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();

            FakeMCPacketReader reader = new FakeMCPacketReader(new VarInt(4));

            Assert.Throws<ArgumentException>(() =>
            {
                deserializer.Deserialize(reader);
            });
        }

    }
}
