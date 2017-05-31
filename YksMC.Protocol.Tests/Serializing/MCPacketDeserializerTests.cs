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
using YksMC.Protocol.Models.Constants;

namespace YksMC.Protocol.Tests.Serializing
{
    [TestFixture]
    public class MCPacketDeserializerTests
    {
        [Test]
        public void Deserialize_HandshakePacket_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            HandshakePacket expected = new HandshakePacket()
            {
                Id = new VarInt(0x00),
                ProtocolVersion = ProtocolVersion.Unknown,
                ServerAddress = "ABC",
                ServerPort = 1234,
                NextState = ConnectionState.Status
            };
            FakeMCPacketReader reader = new FakeMCPacketReader(new VarInt(0x00), new VarInt(-1), "ABC", (ushort)1234, new VarInt(0x01));

            IPacket result = deserializer.Deserialize<HandshakePacket>(reader);

            Assert.IsTrue(ComparisonUtil.Compare(expected, result));
        }

        [Test]
        public void Deserialize_Bool_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(true);

            GenericPacket<bool> result = deserializer.Deserialize<GenericPacket<bool>>(reader);

            Assert.AreEqual(true, result.Value);
        }

        [Test]
        public void Deserialize_SignedByte_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader((sbyte)-5);

            GenericPacket<sbyte> result = deserializer.Deserialize<GenericPacket<sbyte>>(reader);

            Assert.AreEqual(-5, result.Value);
        }

        [Test]
        public void Deserialize_Byte_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader((byte)200);

            GenericPacket<byte> result = deserializer.Deserialize<GenericPacket<byte>>(reader);

            Assert.AreEqual(200, result.Value);
        }

        [Test]
        public void Deserialize_Short_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader((short)-5200);

            GenericPacket<short> result = deserializer.Deserialize<GenericPacket<short>>(reader);

            Assert.AreEqual(-5200, result.Value);
        }

        [Test]
        public void Deserialize_UnsignedShort_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader((ushort)5200);

            GenericPacket<ushort> result = deserializer.Deserialize<GenericPacket<ushort>>(reader);

            Assert.AreEqual(5200, result.Value);
        }

        [Test]
        public void Deserialize_Int_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(int.MinValue);

            GenericPacket<int> result = deserializer.Deserialize<GenericPacket<int>>(reader);

            Assert.AreEqual(int.MinValue, result.Value);
        }

        [Test]
        public void Deserialize_UnsignedInt_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(uint.MaxValue);

            GenericPacket<uint> result = deserializer.Deserialize<GenericPacket<uint>>(reader);

            Assert.AreEqual(uint.MaxValue, result.Value);
        }

        [Test]
        public void Deserialize_Long_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(long.MinValue);

            GenericPacket<long> result = deserializer.Deserialize<GenericPacket<long>>(reader);

            Assert.AreEqual(long.MinValue, result.Value);
        }

        [Test]
        public void Deserialize_UnsignedLong_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(ulong.MaxValue);

            GenericPacket<ulong> result = deserializer.Deserialize<GenericPacket<ulong>>(reader);

            Assert.AreEqual(ulong.MaxValue, result.Value);
        }

        [Test]
        public void Deserialize_Float_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(13.42355f);

            GenericPacket<float> result = deserializer.Deserialize<GenericPacket<float>>(reader);

            Assert.AreEqual(13.42355f, result.Value);
        }

        [Test]
        public void Deserialize_Double_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(232146.123);

            GenericPacket<double> result = deserializer.Deserialize<GenericPacket<double>>(reader);

            Assert.AreEqual(232146.123, result.Value);
        }

        [Test]
        public void Deserialize_String_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader("Test");

            GenericPacket<string> result = deserializer.Deserialize<GenericPacket<string>>(reader);

            Assert.AreEqual("Test", result.Value);
        }

        [Test]
        public void Deserialize_Chat_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(new Chat("Test"));

            GenericPacket<Chat> result = deserializer.Deserialize<GenericPacket<Chat>>(reader);

            Assert.AreEqual(new Chat("Test"), result.Value);
        }

        [Test]
        public void Deserialize_VarInt_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(new VarInt(5));

            GenericPacket<VarInt> result = deserializer.Deserialize<GenericPacket<VarInt>>(reader);

            Assert.AreEqual(new VarInt(5), result.Value);
        }

        [Test]
        public void Deserialize_VarLong_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(new VarLong(-5));

            GenericPacket<VarLong> result = deserializer.Deserialize<GenericPacket<VarLong>>(reader);

            Assert.AreEqual(new VarLong(-5), result.Value);
        }

        [Test]
        public void Deserialize_Position_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(new Position(1, 2, 3));

            GenericPacket<Position> result = deserializer.Deserialize<GenericPacket<Position>>(reader);

            Assert.AreEqual(new Position(1, 2, 3), result.Value);
        }

        [Test]
        public void Deserialize_WithAngle_WritesAngle()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(new Angle(200));

            GenericPacket<Angle> result = deserializer.Deserialize<GenericPacket<Angle>>(reader);

            Assert.AreEqual(new Angle(200), result.Value);
        }

        [Test]
        public void Deserialize_Guid_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            Guid guid = Guid.NewGuid();
            FakeMCPacketReader reader = new FakeMCPacketReader(guid);

            GenericPacket<Guid> result = deserializer.Deserialize<GenericPacket<Guid>>(reader);

            Assert.AreEqual(guid, result.Value);
        }

        [Test]
        public void Deserialize_KnownEnumValue_Works()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(new VarInt((int)ProtocolVersion.Unknown));

            GenericPacket<ProtocolVersion> result = deserializer.Deserialize<GenericPacket<ProtocolVersion>>(reader);

            Assert.AreEqual(ProtocolVersion.Unknown, result.Value);
        }
        [Test]
        public void Deserialize_UnknownEnumValue_Throws()
        {
            MCPacketDeserializer deserializer = new MCPacketDeserializer();
            FakeMCPacketReader reader = new FakeMCPacketReader(new VarInt((int)-12387));

            Assert.Throws<ArgumentException>(() =>
            {
                deserializer.Deserialize<GenericPacket<ProtocolVersion>>(reader);
            });
        }

        [Test]
        public void Deserialize_UnsupportedPropertyType_Throws()
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
