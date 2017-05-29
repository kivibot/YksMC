using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YksMC.Protocol.Tests.Fakes;
using YksMC.Protocol;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Utils;

namespace YksMC.Protocol.Tests
{
    [TestFixture]
    public class MCPacketWriterTests
    {
        [Test]
        public async Task TestSendPacketAsyncWithEmptyPacket()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);

            await writer.SendPacketAsync();

            Assert.AreEqual(new byte[0], connection.SentPackets[0]);
        }


        [Test]
        public async Task TestSendPacketAsyncClearsPreviousData()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);

            writer.PutAngle(new Angle(6));
            await writer.SendPacketAsync();
            await writer.SendPacketAsync();

            Assert.AreEqual(2, connection.SentPackets.Count);
            Assert.AreEqual(1, connection.SentPackets[0].Length);
            Assert.AreEqual(new byte[0], connection.SentPackets[1]);
        }

        [Test]
        public async Task TestPutAngle()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);

            writer.PutAngle(new Angle(200));
            writer.PutAngle(new Angle(1));
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 200, 1 }, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutBool()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);

            writer.PutBool(true);
            writer.PutBool(false);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 1, 0 }, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutSignedByte()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);

            writer.PutSignedByte(-1);
            writer.PutSignedByte(5);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 255, 5 }, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutByte()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);

            writer.PutByte(1);
            writer.PutByte(255);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 1, 255 }, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutShort()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);

            writer.PutShort(-1);
            writer.PutShort(short.MaxValue);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 0xff, 0xff, 0x7f, 0xff }, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutUnsignedShort()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);

            writer.PutUnsignedShort(ushort.MaxValue - 1);
            writer.PutUnsignedShort(1);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 0xff, 0xfe, 0x00, 0x01 }, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutInt()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);

            writer.PutInt(-1);
            writer.PutInt(int.MaxValue);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 0xff, 0xff, 0xff, 0xff, 0x7f, 0xff, 0xff, 0xff }, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutUnsignedInt()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);

            writer.PutUnsignedInt(1);
            writer.PutUnsignedInt(uint.MaxValue);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 0x00, 0x00, 0x00, 0x01, 0xff, 0xff, 0xff, 0xff }, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutLong()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);

            writer.PutLong(-1);
            writer.PutLong(long.MaxValue);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutUnsignedLong()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);

            writer.PutUnsignedLong(1);
            writer.PutUnsignedLong(ulong.MaxValue);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutFloat()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);

            writer.PutFloat(-6);
            writer.PutFloat(9223.3720703125f);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] {
                0xc0, 0xc0, 0x00, 0x00,
                0x46, 0x10, 0x1d, 0x7d
            }, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutDouble()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);

            writer.PutDouble(-6);
            writer.PutDouble(9223.3720703125);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] {
                0xc0, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x40, 0xc2, 0x03, 0xaf, 0xa0, 0x00, 0x00, 0x00,
            }, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutString()
        {
            string str = "TÄMÄ on testi持燃ク禁購ヤホキ前追にゆびト見柳フ止図主のあ。鯉メヤロヒ連降タ誌番アノキロ氏難ア問士こ仰律ぽぱ韓新セチ動呼トは意触げ深学じふたい時Lorem ipsúm dolor sit amet, his cú habeo labítur, eos gloriatur omittantur ad, ex ius solet possim indoctum. Summo vólumus añ mel, sed ex doctus nostrúd, hás eu quis diám sóleat. Eúm at legeré ígnota conclusiónemque. Et meí suavitáte principes. Et sumo éverti quo, ex apeírian mnésarchum temporibus eam. Ad minim quidam sít, verí temporibus hás in.38制ろ問権タメ持掲各ぽーろこ避防覚創ひあ。会むっリ岡都むめびく徳処ミ命6置レの格討ゆ女空をりらに渡1年て予鰒ミツ意組ドるちぼ悪2企ノタヌオ辞4水ヱニヘユ積浩つわんち。";
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);
            List<byte> bytes = new List<byte>();
            bytes.AddRange(new byte[] { 0b10110011, 0b00000110 });
            bytes.AddRange(Encoding.UTF8.GetBytes(str));
            bytes.AddRange(new byte[] { 0 });

            writer.PutString(str);
            writer.PutString("");
            await writer.SendPacketAsync();


            Assert.AreEqual(bytes, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutChat()
        {
            string str = "TÄMÄ on testi持燃ク禁購ヤホキ前追にゆびト見柳フ止図主のあ。鯉メヤロヒ連降タ誌番アノキロ氏難ア問士こ仰律ぽぱ韓新セチ動呼トは意触げ深学じふたい時Lorem ipsúm dolor sit amet, his cú habeo labítur, eos gloriatur omittantur ad, ex ius solet possim indoctum. Summo vólumus añ mel, sed ex doctus nostrúd, hás eu quis diám sóleat. Eúm at legeré ígnota conclusiónemque. Et meí suavitáte principes. Et sumo éverti quo, ex apeírian mnésarchum temporibus eam. Ad minim quidam sít, verí temporibus hás in.38制ろ問権タメ持掲各ぽーろこ避防覚創ひあ。会むっリ岡都むめびく徳処ミ命6置レの格討ゆ女空をりらに渡1年て予鰒ミツ意組ドるちぼ悪2企ノタヌオ辞4水ヱニヘユ積浩つわんち。";
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);
            List<byte> bytes = new List<byte>();
            bytes.AddRange(new byte[] { 0b10110011, 0b00000110 });
            bytes.AddRange(Encoding.UTF8.GetBytes(str));
            bytes.AddRange(new byte[] { 0 });

            writer.PutChat(new Chat(str));
            writer.PutChat(new Chat(""));
            await writer.SendPacketAsync();


            Assert.AreEqual(bytes, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutVarInt()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);
            List<byte> bytes = new List<byte>();
            bytes.AddRange(VarIntUtil.EncodeVarInt(-1));
            bytes.AddRange(VarIntUtil.EncodeVarInt(12398));

            writer.PutVarInt(new VarInt(-1));
            writer.PutVarInt(new VarInt(12398));
            await writer.SendPacketAsync();


            Assert.AreEqual(bytes, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutVarLong()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);
            List<byte> bytes = new List<byte>();
            bytes.AddRange(VarIntUtil.EncodeVarLong(-1));
            bytes.AddRange(VarIntUtil.EncodeVarLong(12399));

            writer.PutVarLong(new VarLong(-1));
            writer.PutVarLong(new VarLong(12399));
            await writer.SendPacketAsync();


            Assert.AreEqual(bytes, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutPosition()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);

            writer.PutPosition(new Position(0b01111111111111111111111111, 0b111111111111, 0b11111111111111111111111101));
            writer.PutPosition(new Position(0b01111111111111111111111111, 0b111111111111, 0b11111111111111111111111110));
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] {
                0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfd,
                0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfe
            }, connection.SentPackets[0]);
        }

        [Test]
        public async Task TestPutGuid()
        {
            FakeMCConnection connection = new FakeMCConnection();
            MCPacketWriter writer = new MCPacketWriter(connection);
            List<byte> bytes = new List<byte>();
            Guid testGuid = Guid.NewGuid();
            bytes.AddRange(Guid.Empty.ToByteArray().Reverse());
            bytes.AddRange(testGuid.ToByteArray().Reverse());

            writer.PutGuid(Guid.Empty);
            writer.PutGuid(testGuid);
            await writer.SendPacketAsync();


            Assert.AreEqual(bytes, connection.SentPackets[0]);
        }

    }
}
