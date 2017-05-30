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
    public class MCPacketBuilderTests
    {
        [Test]
        public void TakePacket_NoData_ReturnsEmptyArray()
        {
            MCPacketBuilder builder = new MCPacketBuilder();

            byte[] result = builder.TakePacket();

            Assert.AreEqual(new byte[0], result);
        }

        [Test]
        public void TakePacket_ValidData_ClearsBuffers()
        {
            MCPacketBuilder builder = new MCPacketBuilder();

            builder.PutString("TEST");
            builder.TakePacket();
            byte[] result = builder.TakePacket();

            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void PutAngle_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();

            builder.PutAngle(new Angle(200));
            builder.PutAngle(new Angle(1));
            byte[] result = builder.TakePacket();


            Assert.AreEqual(new byte[] { 200, 1 }, result);
        }

        [Test]
        public void PutByte_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();

            builder.PutBool(true);
            builder.PutBool(false);
            byte[] result = builder.TakePacket();


            Assert.AreEqual(new byte[] { 1, 0 }, result);
        }

        [Test]
        public void PutSignedByte_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();

            builder.PutSignedByte(-1);
            builder.PutSignedByte(5);
            byte[] result = builder.TakePacket();


            Assert.AreEqual(new byte[] { 255, 5 }, result);
        }

        [Test]
        public void PytByte_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();

            builder.PutByte(1);
            builder.PutByte(255);
            byte[] result = builder.TakePacket();


            Assert.AreEqual(new byte[] { 1, 255 }, result);
        }

        [Test]
        public void PutShort_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();

            builder.PutShort(-1);
            builder.PutShort(short.MaxValue);
            byte[] result = builder.TakePacket();


            Assert.AreEqual(new byte[] { 0xff, 0xff, 0x7f, 0xff }, result);
        }

        [Test]
        public void PutUnsignedShort_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();

            builder.PutUnsignedShort(ushort.MaxValue - 1);
            builder.PutUnsignedShort(1);
            byte[] result = builder.TakePacket();


            Assert.AreEqual(new byte[] { 0xff, 0xfe, 0x00, 0x01 }, result);
        }

        [Test]
        public void PutInt_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();

            builder.PutInt(-1);
            builder.PutInt(int.MaxValue);
            byte[] result = builder.TakePacket();


            Assert.AreEqual(new byte[] { 0xff, 0xff, 0xff, 0xff, 0x7f, 0xff, 0xff, 0xff }, result);
        }

        [Test]
        public void PutUnsignedInt_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();

            builder.PutUnsignedInt(1);
            builder.PutUnsignedInt(uint.MaxValue);
            byte[] result = builder.TakePacket();


            Assert.AreEqual(new byte[] { 0x00, 0x00, 0x00, 0x01, 0xff, 0xff, 0xff, 0xff }, result);
        }

        [Test]
        public void PutLong_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();

            builder.PutLong(-1);
            builder.PutLong(long.MaxValue);
            byte[] result = builder.TakePacket();


            Assert.AreEqual(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, result);
        }

        [Test]
        public void PutUnsignedLong_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();

            builder.PutUnsignedLong(1);
            builder.PutUnsignedLong(ulong.MaxValue);
            byte[] result = builder.TakePacket();


            Assert.AreEqual(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff }, result);
        }

        [Test]
        public void PutFloat_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();

            builder.PutFloat(-6);
            builder.PutFloat(9223.3720703125f);
            byte[] result = builder.TakePacket();


            Assert.AreEqual(new byte[] {
                0xc0, 0xc0, 0x00, 0x00,
                0x46, 0x10, 0x1d, 0x7d
            }, result);
        }

        [Test]
        public void PutDouble_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();

            builder.PutDouble(-6);
            builder.PutDouble(9223.3720703125);
            byte[] result = builder.TakePacket();


            Assert.AreEqual(new byte[] {
                0xc0, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x40, 0xc2, 0x03, 0xaf, 0xa0, 0x00, 0x00, 0x00,
            }, result);
        }

        [Test]
        public void PutString_ValidData_AppendsCorrectData()
        {
            string str = "TÄMÄ on testi持燃ク禁購ヤホキ前追にゆびト見柳フ止図主のあ。鯉メヤロヒ連降タ誌番アノキロ氏難ア問士こ仰律ぽぱ韓新セチ動呼トは意触げ深学じふたい時Lorem ipsúm dolor sit amet, his cú habeo labítur, eos gloriatur omittantur ad, ex ius solet possim indoctum. Summo vólumus añ mel, sed ex doctus nostrúd, hás eu quis diám sóleat. Eúm at legeré ígnota conclusiónemque. Et meí suavitáte principes. Et sumo éverti quo, ex apeírian mnésarchum temporibus eam. Ad minim quidam sít, verí temporibus hás in.38制ろ問権タメ持掲各ぽーろこ避防覚創ひあ。会むっリ岡都むめびく徳処ミ命6置レの格討ゆ女空をりらに渡1年て予鰒ミツ意組ドるちぼ悪2企ノタヌオ辞4水ヱニヘユ積浩つわんち。";
            MCPacketBuilder builder = new MCPacketBuilder();
            List<byte> bytes = new List<byte>();
            bytes.AddRange(new byte[] { 0b10110011, 0b00000110 });
            bytes.AddRange(Encoding.UTF8.GetBytes(str));
            bytes.AddRange(new byte[] { 0 });

            builder.PutString(str);
            builder.PutString("");
            byte[] result = builder.TakePacket();


            Assert.AreEqual(bytes, result);
        }

        [Test]
        public void PutChat_ValidData_AppendsCorrectData()
        {
            string str = "TÄMÄ on testi持燃ク禁購ヤホキ前追にゆびト見柳フ止図主のあ。鯉メヤロヒ連降タ誌番アノキロ氏難ア問士こ仰律ぽぱ韓新セチ動呼トは意触げ深学じふたい時Lorem ipsúm dolor sit amet, his cú habeo labítur, eos gloriatur omittantur ad, ex ius solet possim indoctum. Summo vólumus añ mel, sed ex doctus nostrúd, hás eu quis diám sóleat. Eúm at legeré ígnota conclusiónemque. Et meí suavitáte principes. Et sumo éverti quo, ex apeírian mnésarchum temporibus eam. Ad minim quidam sít, verí temporibus hás in.38制ろ問権タメ持掲各ぽーろこ避防覚創ひあ。会むっリ岡都むめびく徳処ミ命6置レの格討ゆ女空をりらに渡1年て予鰒ミツ意組ドるちぼ悪2企ノタヌオ辞4水ヱニヘユ積浩つわんち。";
            MCPacketBuilder builder = new MCPacketBuilder();
            List<byte> bytes = new List<byte>();
            bytes.AddRange(new byte[] { 0b10110011, 0b00000110 });
            bytes.AddRange(Encoding.UTF8.GetBytes(str));
            bytes.AddRange(new byte[] { 0 });

            builder.PutChat(new Chat(str));
            builder.PutChat(new Chat(""));
            byte[] result = builder.TakePacket();


            Assert.AreEqual(bytes, result);
        }

        [Test]
        public void PutVarInt_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();
            List<byte> bytes = new List<byte>();
            bytes.AddRange(VarIntUtil.EncodeVarInt(-1));
            bytes.AddRange(VarIntUtil.EncodeVarInt(12398));

            builder.PutVarInt(new VarInt(-1));
            builder.PutVarInt(new VarInt(12398));
            byte[] result = builder.TakePacket();


            Assert.AreEqual(bytes, result);
        }

        [Test]
        public void PutVarLong_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();
            List<byte> bytes = new List<byte>();
            bytes.AddRange(VarIntUtil.EncodeVarLong(-1));
            bytes.AddRange(VarIntUtil.EncodeVarLong(12399));

            builder.PutVarLong(new VarLong(-1));
            builder.PutVarLong(new VarLong(12399));
            byte[] result = builder.TakePacket();


            Assert.AreEqual(bytes, result);
        }

        [Test]
        public void PutPosition_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();

            builder.PutPosition(new Position(0b01111111111111111111111111, 0b111111111111, 0b11111111111111111111111101));
            builder.PutPosition(new Position(0b01111111111111111111111111, 0b111111111111, 0b11111111111111111111111110));
            byte[] result = builder.TakePacket();


            Assert.AreEqual(new byte[] {
                0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfd,
                0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfe
            }, result);
        }

        [Test]
        public void PutGuid_ValidData_AppendsCorrectData()
        {
            MCPacketBuilder builder = new MCPacketBuilder();
            List<byte> bytes = new List<byte>();
            Guid testGuid = Guid.NewGuid();
            bytes.AddRange(Guid.Empty.ToByteArray().Reverse());
            bytes.AddRange(testGuid.ToByteArray().Reverse());

            builder.PutGuid(Guid.Empty);
            builder.PutGuid(testGuid);
            byte[] result = builder.TakePacket();


            Assert.AreEqual(bytes, result);
        }

    }
}
