using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Protocol.Tests.Fakes;
using YksMC.Protocol.Tests.Models;
using YksMC.Protocol.Tests.TestUtils;
using YksMC.Protocol;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Serializing;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Packets;
using YksMC.Protocol.Models;

namespace YksMC.Protocol.Tests.Serializing
{
    [TestFixture]
    public class PacketDeserializerTests
    {
        [Test]
        public void Deserialize_HandshakePacket_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            HandshakePacket expected = new HandshakePacket()
            {
                ProtocolVersion = ProtocolVersion.Unknown,
                ServerAddress = "ABC",
                ServerPort = 1234,
                NextState = ConnectionState.Status
            };
            FakePacketReader reader = new FakePacketReader(new VarInt(-1), "ABC", (ushort)1234, new VarInt(0x01));

            object result = deserializer.Deserialize<HandshakePacket>(reader);

            Assert.IsTrue(ComparisonUtil.Compare(expected, result));
        }

        [Test]
        public void Deserialize_Bool_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(true);

            GenericPacket<bool> result = deserializer.Deserialize<GenericPacket<bool>>(reader);

            Assert.AreEqual(true, result.Value);
        }

        [Test]
        public void Deserialize_SignedByte_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader((sbyte)-5);

            GenericPacket<sbyte> result = deserializer.Deserialize<GenericPacket<sbyte>>(reader);

            Assert.AreEqual(-5, result.Value);
        }

        [Test]
        public void Deserialize_Byte_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader((byte)200);

            GenericPacket<byte> result = deserializer.Deserialize<GenericPacket<byte>>(reader);

            Assert.AreEqual(200, result.Value);
        }

        [Test]
        public void Deserialize_Short_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader((short)-5200);

            GenericPacket<short> result = deserializer.Deserialize<GenericPacket<short>>(reader);

            Assert.AreEqual(-5200, result.Value);
        }

        [Test]
        public void Deserialize_UnsignedShort_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader((ushort)5200);

            GenericPacket<ushort> result = deserializer.Deserialize<GenericPacket<ushort>>(reader);

            Assert.AreEqual(5200, result.Value);
        }

        [Test]
        public void Deserialize_Int_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(int.MinValue);

            GenericPacket<int> result = deserializer.Deserialize<GenericPacket<int>>(reader);

            Assert.AreEqual(int.MinValue, result.Value);
        }

        [Test]
        public void Deserialize_UnsignedInt_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(uint.MaxValue);

            GenericPacket<uint> result = deserializer.Deserialize<GenericPacket<uint>>(reader);

            Assert.AreEqual(uint.MaxValue, result.Value);
        }

        [Test]
        public void Deserialize_Long_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(long.MinValue);

            GenericPacket<long> result = deserializer.Deserialize<GenericPacket<long>>(reader);

            Assert.AreEqual(long.MinValue, result.Value);
        }

        [Test]
        public void Deserialize_UnsignedLong_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(ulong.MaxValue);

            GenericPacket<ulong> result = deserializer.Deserialize<GenericPacket<ulong>>(reader);

            Assert.AreEqual(ulong.MaxValue, result.Value);
        }

        [Test]
        public void Deserialize_Float_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(13.42355f);

            GenericPacket<float> result = deserializer.Deserialize<GenericPacket<float>>(reader);

            Assert.AreEqual(13.42355f, result.Value);
        }

        [Test]
        public void Deserialize_Double_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(232146.123);

            GenericPacket<double> result = deserializer.Deserialize<GenericPacket<double>>(reader);

            Assert.AreEqual(232146.123, result.Value);
        }

        [Test]
        public void Deserialize_String_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader("Test");

            GenericPacket<string> result = deserializer.Deserialize<GenericPacket<string>>(reader);

            Assert.AreEqual("Test", result.Value);
        }

        [Test]
        public void Deserialize_Chat_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(new Chat("Test"));

            GenericPacket<Chat> result = deserializer.Deserialize<GenericPacket<Chat>>(reader);

            Assert.AreEqual(new Chat("Test"), result.Value);
        }

        [Test]
        public void Deserialize_VarInt_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(new VarInt(5));

            GenericPacket<VarInt> result = deserializer.Deserialize<GenericPacket<VarInt>>(reader);

            Assert.AreEqual(new VarInt(5), result.Value);
        }

        [Test]
        public void Deserialize_VarLong_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(new VarLong(-5));

            GenericPacket<VarLong> result = deserializer.Deserialize<GenericPacket<VarLong>>(reader);

            Assert.AreEqual(new VarLong(-5), result.Value);
        }

        [Test]
        public void Deserialize_Position_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(new Position(1, 2, 3));

            GenericPacket<Position> result = deserializer.Deserialize<GenericPacket<Position>>(reader);

            Assert.AreEqual(new Position(1, 2, 3), result.Value);
        }

        [Test]
        public void Deserialize_WithAngle_WritesAngle()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(new Angle(200));

            GenericPacket<Angle> result = deserializer.Deserialize<GenericPacket<Angle>>(reader);

            Assert.AreEqual(new Angle(200), result.Value);
        }

        [Test]
        public void Deserialize_Guid_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            Guid guid = Guid.NewGuid();
            FakePacketReader reader = new FakePacketReader(guid);

            GenericPacket<Guid> result = deserializer.Deserialize<GenericPacket<Guid>>(reader);

            Assert.AreEqual(guid, result.Value);
        }

        [Test]
        public void Deserialize_KnownEnumValue_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(new VarInt((int)ProtocolVersion.Unknown));

            GenericPacket<ProtocolVersion> result = deserializer.Deserialize<GenericPacket<ProtocolVersion>>(reader);

            Assert.AreEqual(ProtocolVersion.Unknown, result.Value);
        }

        [Test]
        public void Deserialize_UnknownEnumValue_Throws()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(new VarInt((int)-12387));

            Assert.Throws<ArgumentException>(() =>
            {
                deserializer.Deserialize<GenericPacket<ProtocolVersion>>(reader);
            });
        }

        [Test]
        public void Deserialize_UnsupportedPropertyType_Throws()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader();

            Assert.Throws<ArgumentException>(() =>
            {
                deserializer.Deserialize<GenericPacket<DateTime>>(reader);
            });
        }

        [Test]
        public void Deserialize_IntegerVarArray_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(new VarInt(3), (int)0, (int)0, (int)0);

            GenericPacket<VarArray<VarInt, int>> result = deserializer.Deserialize<GenericPacket<VarArray<VarInt, int>>>(reader);

            Assert.AreEqual(new VarArray<VarInt, int>() { Count = new VarInt(3), Values = new int[3] }, result.Value);
        }

        [Test]
        public void Deserialize_ByteVarArray_UsesByteArrays()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(new VarInt(3), new byte[3]);

            GenericPacket<VarArray<VarInt, byte>> result = deserializer.Deserialize<GenericPacket<VarArray<VarInt, byte>>>(reader);

            Assert.AreEqual(new VarArray<VarInt, byte>() { Count = new VarInt(3), Values = new byte[3] }, result.Value);
        }

        [Test]
        public void Deserialize_Object_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader("Test");

            GenericPacket<GenericPacket<string>> result = deserializer.Deserialize<GenericPacket<GenericPacket<string>>>(reader);

            Assert.AreEqual("Test", result.Value.Value);
        }

        [Test]
        public void Deserialize_OptionalWithoutValue_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(false);

            Optional<string> result = deserializer.Deserialize<Optional<string>>(reader);

            Assert.AreEqual(new Optional<string>(), result);
        }

        [Test]
        public void Deserialize_OptionalWithValue_Works()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(true, ":D");

            Optional<string> result = deserializer.Deserialize<Optional<string>>(reader);

            Assert.AreEqual(new Optional<string>() { HasValue = true, Value = ":D" }, result);
        }

        [Test]
        public void Deserialize_IsConditionalWithValue_DeserialiazesIntValue()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(2, ":D", 5);

            ConditionalPacket result = deserializer.Deserialize<ConditionalPacket>(reader);

            Assert.AreEqual(5, result.IntValue);
        }

        [Test]
        public void Deserialize_IsConditionalWithoutValue_DoesNotDeserialize()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(1, ":D", "");

            ConditionalPacket result = deserializer.Deserialize<ConditionalPacket>(reader);

            Assert.AreEqual(default(int), result.IntValue);
        }

        [Test]
        public void Deserialize_IsNotConditionalWithValue_DoesNotDeserialize()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(2, ":D", 6);

            ConditionalPacket result = deserializer.Deserialize<ConditionalPacket>(reader);

            Assert.AreEqual(null, result.StringValue);
        }

        [Test]
        public void Deserialize_IsNotConditionalWithoutValue_DeserializedStringValue()
        {
            PacketDeserializer deserializer = new PacketDeserializer();
            FakePacketReader reader = new FakePacketReader(0, ":D", "Test");

            ConditionalPacket result = deserializer.Deserialize<ConditionalPacket>(reader);

            Assert.AreEqual("Test", result.StringValue);
        }

    }
}
