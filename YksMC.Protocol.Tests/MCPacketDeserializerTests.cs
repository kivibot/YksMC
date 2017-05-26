using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Protocol.Tests.Fakes;
using YksMC.Protocol.Tests.Models;
using YksMC.Protocol.Tests.TestUtils;
using YksMC.Protocol;
using YksMC.Protocol.Models.Packets;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Tests
{
    [TestFixture]
    public class MCPacketDeserializerTests
    {
        [Test]
        public void TestDeserializeHandshakePacket()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            HandshakePacket expected = new HandshakePacket()
            {
                ProtocolVersion = new VarInt(0x12),
                ServerAddress = "ABC",
                ServerPort = 1234,
                NextState = new VarInt(0x01)
            };
            FakeMCPacketReader reader = new FakeMCPacketReader(new VarInt(0x12), "ABC", (ushort)1234, new VarInt(0x01));

            AbstractPacket result = deserializer.Deserialize<HandshakePacket>(reader);

            Assert.IsTrue(ComparisonUtil.Compare(expected, result));
        }

        [Test]
        public void TestDeserializeCanDeserializeAllTypes()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            TestPacket expected = new TestPacket()
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
            FakeMCPacketReader reader = new FakeMCPacketReader(expected.Bool, expected.SignedByte, expected.Byte, expected.Short, expected.UnsignedShort,
                expected.Int, expected.UnsignedInt, expected.Long, expected.UnsignedLong, expected.Float, expected.Double, expected.String, expected.Chat,
                expected.VarInt, expected.VarLong, expected.Position, expected.Angle, expected.Guid);

            AbstractPacket result = deserializer.Deserialize<TestPacket>(reader);

            Assert.IsTrue(ComparisonUtil.Compare(expected, result));
        }

        [Test]
        public void TestDeserializeDoesNotAcceptUnsupportedProperties()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader();

            Assert.Throws<ArgumentException>(() =>
            {
                deserializer.Deserialize<InvalidPacket>(reader);
            });
        }

    }
}
