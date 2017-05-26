using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YksMc.Protocol.Tests.Fakes;
using YksMC.Protocol;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Utils;

namespace YksMc.Protocol.Tests
{
    [TestFixture]
    public class MCPacketWriterTests
    {
        [Test]
        public async Task TestSendPacketAsyncWithEmptyPacket()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);

            await writer.SendPacketAsync();

            Assert.AreEqual(new byte[0], sink.Packets[0]);
        }


        [Test]
        public async Task TestSendPacketAsyncClearsPreviousData()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);

            writer.PutAngle(new Angle(6));
            await writer.SendPacketAsync();
            await writer.SendPacketAsync();

            Assert.AreEqual(2, sink.Packets.Count);
            Assert.AreEqual(1, sink.Packets[0].Length);
            Assert.AreEqual(new byte[0], sink.Packets[1]);
        }

        [Test]
        public async Task TestPutAngle()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);

            writer.PutAngle(new Angle(200));
            writer.PutAngle(new Angle(1));
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 200, 1 }, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutBool()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);

            writer.PutBool(true);
            writer.PutBool(false);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 1, 0 }, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutSignedByte()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);

            writer.PutSignedByte(-1);
            writer.PutSignedByte(5);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 255, 5 }, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutByte()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);

            writer.PutByte(1);
            writer.PutByte(255);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 1, 255 }, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutShort()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);

            writer.PutShort(-1);
            writer.PutShort(short.MaxValue);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 0xff, 0xff, 0x7f, 0xff }, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutUnsignedShort()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);

            writer.PutUnsignedShort(ushort.MaxValue - 1);
            writer.PutUnsignedShort(1);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 0xff, 0xfe, 0x00, 0x01 }, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutInt()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);

            writer.PutInt(-1);
            writer.PutInt(int.MaxValue);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 0xff, 0xff, 0xff, 0xff, 0x7f, 0xff, 0xff, 0xff }, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutUnsignedInt()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);

            writer.PutUnsignedInt(1);
            writer.PutUnsignedInt(uint.MaxValue);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 0x00, 0x00, 0x00, 0x01, 0xff, 0xff, 0xff, 0xff }, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutLong()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);

            writer.PutLong(-1);
            writer.PutLong(long.MaxValue);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutUnsignedLong()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);

            writer.PutUnsignedLong(1);
            writer.PutUnsignedLong(ulong.MaxValue);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutFloat()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);

            writer.PutFloat(-6);
            writer.PutFloat(9223.3720703125f);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] {
                0xc0, 0xc0, 0x00, 0x00,
                0x46, 0x10, 0x1d, 0x7d
            }, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutDouble()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);

            writer.PutDouble(-6);
            writer.PutDouble(9223.3720703125);
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] {
                0xc0, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x40, 0xc2, 0x03, 0xaf, 0xa0, 0x00, 0x00, 0x00,
            }, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutString()
        {
            string str = "TÄMÄ on testi持燃ク禁購ヤホキ前追にゆびト見柳フ止図主のあ。鯉メヤロヒ連降タ誌番アノキロ氏難ア問士こ仰律ぽぱ韓新セチ動呼トは意触げ深学じふたい時Lorem ipsúm dolor sit amet, his cú habeo labítur, eos gloriatur omittantur ad, ex ius solet possim indoctum. Summo vólumus añ mel, sed ex doctus nostrúd, hás eu quis diám sóleat. Eúm at legeré ígnota conclusiónemque. Et meí suavitáte principes. Et sumo éverti quo, ex apeírian mnésarchum temporibus eam. Ad minim quidam sít, verí temporibus hás in.38制ろ問権タメ持掲各ぽーろこ避防覚創ひあ。会むっリ岡都むめびく徳処ミ命6置レの格討ゆ女空をりらに渡1年て予鰒ミツ意組ドるちぼ悪2企ノタヌオ辞4水ヱニヘユ積浩つわんち。";
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);
            List<byte> bytes = new List<byte>();
            bytes.AddRange(new byte[] { 0b10110011, 0b00000110 });
            bytes.AddRange(Encoding.UTF8.GetBytes(str));
            bytes.AddRange(new byte[] { 0 });

            writer.PutString(str);
            writer.PutString("");
            await writer.SendPacketAsync();


            Assert.AreEqual(bytes, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutChat()
        {
            string str = "TÄMÄ on testi持燃ク禁購ヤホキ前追にゆびト見柳フ止図主のあ。鯉メヤロヒ連降タ誌番アノキロ氏難ア問士こ仰律ぽぱ韓新セチ動呼トは意触げ深学じふたい時Lorem ipsúm dolor sit amet, his cú habeo labítur, eos gloriatur omittantur ad, ex ius solet possim indoctum. Summo vólumus añ mel, sed ex doctus nostrúd, hás eu quis diám sóleat. Eúm at legeré ígnota conclusiónemque. Et meí suavitáte principes. Et sumo éverti quo, ex apeírian mnésarchum temporibus eam. Ad minim quidam sít, verí temporibus hás in.38制ろ問権タメ持掲各ぽーろこ避防覚創ひあ。会むっリ岡都むめびく徳処ミ命6置レの格討ゆ女空をりらに渡1年て予鰒ミツ意組ドるちぼ悪2企ノタヌオ辞4水ヱニヘユ積浩つわんち。";
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);
            List<byte> bytes = new List<byte>();
            bytes.AddRange(new byte[] { 0b10110011, 0b00000110 });
            bytes.AddRange(Encoding.UTF8.GetBytes(str));
            bytes.AddRange(new byte[] { 0 });

            writer.PutChat(new Chat(str));
            writer.PutChat(new Chat(""));
            await writer.SendPacketAsync();


            Assert.AreEqual(bytes, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutVarInt()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);
            List<byte> bytes = new List<byte>();
            bytes.AddRange(VarIntUtil.EncodeVarInt(-1));
            bytes.AddRange(VarIntUtil.EncodeVarInt(12398));

            writer.PutVarInt(new VarInt(-1));
            writer.PutVarInt(new VarInt(12398));
            await writer.SendPacketAsync();


            Assert.AreEqual(bytes, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutVarLong()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);
            List<byte> bytes = new List<byte>();
            bytes.AddRange(VarIntUtil.EncodeVarLong(-1));
            bytes.AddRange(VarIntUtil.EncodeVarLong(12399));

            writer.PutVarLong(new VarLong(-1));
            writer.PutVarLong(new VarLong(12399));
            await writer.SendPacketAsync();


            Assert.AreEqual(bytes, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutPosition()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);

            writer.PutPosition(new Position(0b01111111111111111111111111, 0b111111111111, 0b11111111111111111111111101));
            writer.PutPosition(new Position(0b01111111111111111111111111, 0b111111111111, 0b11111111111111111111111110));
            await writer.SendPacketAsync();


            Assert.AreEqual(new byte[] {
                0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfd,
                0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfe
            }, sink.Packets[0]);
        }

        [Test]
        public async Task TestPutGuid()
        {
            FakeMCPacketSink sink = new FakeMCPacketSink();
            MCPacketWriter writer = new MCPacketWriter(sink);
            List<byte> bytes = new List<byte>();
            Guid testGuid = Guid.NewGuid();
            bytes.AddRange(Guid.Empty.ToByteArray().Reverse());
            bytes.AddRange(testGuid.ToByteArray().Reverse());

            writer.PutGuid(Guid.Empty);
            writer.PutGuid(testGuid);
            await writer.SendPacketAsync();


            Assert.AreEqual(bytes, sink.Packets[0]);
        }

    }
}
