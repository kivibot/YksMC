using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMc.Protocol.Tests.Fakes;
using YksMc.Protocol.Tests.Models;
using YksMC.MCProtocol;
using YksMC.MCProtocol.Models.Packets;
using YksMC.MCProtocol.Models.Types;

namespace YksMc.Protocol.Tests
{
    [TestFixture]
    public class MCPacketSerializerTests
    {

        [Test]
        public async Task TestSerializeHandshakePacket()
        {
            MCPacketSerializer serializer = new MCPacketSerializer();
            HandshakePacket packet = new HandshakePacket()
            {
                ProtocolVersion = new VarInt(0x12),
                ServerAddress = "ABC",
                ServerPort = 1234,
                NextState = new VarInt(0x01)
            };
            FakeMCPacketWriter writer = new FakeMCPacketWriter();

            serializer.Serialize(packet, writer);
            await writer.SendPacketAsync();

            Assert.AreEqual(new object[] { new VarInt(0x12), "ABC", (ushort)1234, new VarInt(0x01) }, writer.Objects);
        }

        [Test]
        public async Task TestSerializeSupportsAllTypes()
        {
            MCPacketSerializer serializer = new MCPacketSerializer();
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
            FakeMCPacketWriter writer = new FakeMCPacketWriter();

            serializer.Serialize(packet, writer);
            await writer.SendPacketAsync();

            Assert.AreEqual(new object[] { packet.Bool, packet.SignedByte, packet.Byte, packet.Short, packet.UnsignedShort,
                packet.Int, packet.UnsignedInt, packet.Long, packet.UnsignedLong, packet.Float, packet.Double, packet.String, packet.Chat,
                packet.VarInt, packet.VarLong, packet.Position, packet.Angle, packet.Guid }, writer.Objects);
        }

        [Test]
        public void TestSerializeDoesNotAcceptUnsupportedProperties()
        {
            MCPacketSerializer serializer = new MCPacketSerializer();
            InvalidPacket packet = new InvalidPacket();
            FakeMCPacketWriter writer = new FakeMCPacketWriter();

            Assert.Throws<ArgumentException>(() =>
            {
                serializer.Serialize(packet, writer);
            });
        }

    }
}
