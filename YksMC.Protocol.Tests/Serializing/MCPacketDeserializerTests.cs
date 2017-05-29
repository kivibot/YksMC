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
using YksMC.Protocol.Serializing;

namespace YksMC.Protocol.Tests.Serializing
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
        public void TestDeserializeSupportsBool()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(true);

            GenericPacket<bool> result = deserializer.Deserialize<GenericPacket<bool>>(reader);

            Assert.AreEqual(true, result.Value);
        }

        [Test]
        public void TestDeserializeSupportsSignedByte()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader((sbyte)-5);

            GenericPacket<sbyte> result = deserializer.Deserialize<GenericPacket<sbyte>>(reader);

            Assert.AreEqual(-5, result.Value);
        }

        [Test]
        public void TestDeserializeSupportsByte()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader((byte)200);

            GenericPacket<byte> result = deserializer.Deserialize<GenericPacket<byte>>(reader);

            Assert.AreEqual(200, result.Value);
        }

        [Test]
        public void TestDeserializeSupportsShort()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader((short)-5200);

            GenericPacket<short> result = deserializer.Deserialize<GenericPacket<short>>(reader);

            Assert.AreEqual(-5200, result.Value);
        }

        [Test]
        public void TestDeserializeSupportsUnsignedShort()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader((ushort)5200);

            GenericPacket<ushort> result = deserializer.Deserialize<GenericPacket<ushort>>(reader);

            Assert.AreEqual(5200, result.Value);
        }

        [Test]
        public void TestDeserializeSupportsInt()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(int.MinValue);

            GenericPacket<int> result = deserializer.Deserialize<GenericPacket<int>>(reader);

            Assert.AreEqual(int.MinValue, result.Value);
        }

        [Test]
        public void TestDeserializeSupportsUnsignedInt()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(uint.MaxValue);

            GenericPacket<uint> result = deserializer.Deserialize<GenericPacket<uint>>(reader);

            Assert.AreEqual(uint.MaxValue, result.Value);
        }

        [Test]
        public void TestDeserializeSupportsLong()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(long.MinValue);

            GenericPacket<long> result = deserializer.Deserialize<GenericPacket<long>>(reader);

            Assert.AreEqual(long.MinValue, result.Value);
        }

        [Test]
        public void TestDeserializeSupportsUnsignedLong()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(ulong.MaxValue);

            GenericPacket<ulong> result = deserializer.Deserialize<GenericPacket<ulong>>(reader);

            Assert.AreEqual(ulong.MaxValue, result.Value);
        }

        [Test]
        public void TestDeserializeSupportsFloat()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(13.42355f);

            GenericPacket<float> result = deserializer.Deserialize<GenericPacket<float>>(reader);

            Assert.AreEqual(13.42355f, result.Value);
        }

        [Test]
        public void TestDeserializeSupportsDouble()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(232146.123);

            GenericPacket<double> result = deserializer.Deserialize<GenericPacket<double>>(reader);

            Assert.AreEqual(232146.123, result.Value);
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
