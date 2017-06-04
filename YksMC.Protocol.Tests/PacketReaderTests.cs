using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Protocol.Tests.Fakes;
using YksMC.Protocol;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Utils;

namespace YksMC.Protocol.Tests
{
    [TestFixture]
    public class PacketReaderTests
    {
        [Test]
        public void SetPacket_ExistingPacket_PreviousDataCleared()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] { 1, 2 });
            reader.SetPacket(new byte[] { 3 });
            byte result = reader.GetByte();

            Assert.AreEqual(3, result);
        }

        [Test]
        public void GetBool_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] { 0, 1 });

            bool result0 = reader.GetBool();
            bool result1 = reader.GetBool();

            Assert.AreEqual(false, result0);
            Assert.AreEqual(true, result1);
        }

        [Test]
        public void GetSignedByte_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] { 1, 250 });

            sbyte result0 = reader.GetSignedByte();
            sbyte result1 = reader.GetSignedByte();

            Assert.AreEqual(1, result0);
            Assert.AreEqual(-6, result1);
        }

        [Test]
        public void GetByte_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] { 1, 250 });

            byte result0 = reader.GetByte();
            byte result1 = reader.GetByte();

            Assert.AreEqual(1, result0);
            Assert.AreEqual(250, result1);
        }

        [Test]
        public void GetShort_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] { 0x80, 0x01, 0x7f, 0xff });

            short result0 = reader.GetShort();
            short result1 = reader.GetShort();

            Assert.AreEqual(-32767, result0);
            Assert.AreEqual(32767, result1);
        }

        [Test]
        public void GetUnsignedShort_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] { 0x00, 0x03, 0xa7, 0x0f });

            ushort result0 = reader.GetUnsignedShort();
            ushort result1 = reader.GetUnsignedShort();

            Assert.AreEqual(3, result0);
            Assert.AreEqual(42767, result1);
        }

        [Test]
        public void GetInt_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] { 0xff, 0xff, 0xff, 0xfb, 0x7f, 0xff, 0xff, 0xff });

            int result0 = reader.GetInt();
            int result1 = reader.GetInt();

            Assert.AreEqual(-5, result0);
            Assert.AreEqual(2147483647, result1);
        }

        [Test]
        public void GetUnsignedInt_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] { 0xff, 0xff, 0xff, 0xfb, 0x7f, 0xff, 0xff, 0xff });

            uint result0 = reader.GetUnsignedInt();
            uint result1 = reader.GetUnsignedInt();

            Assert.AreEqual(4294967291, result0);
            Assert.AreEqual(2147483647, result1);
        }

        [Test]
        public void GetLong_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] {
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfb,
                0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
            });

            long result0 = reader.GetLong();
            long result1 = reader.GetLong();

            Assert.AreEqual(-5, result0);
            Assert.AreEqual(9223372036854775807, result1);
        }

        [Test]
        public void GetUnsignedLong_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] {
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfb,
                0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
            });

            ulong result0 = reader.GetUnsignedLong();
            ulong result1 = reader.GetUnsignedLong();

            Assert.AreEqual(ulong.MaxValue - 4, result0);
            Assert.AreEqual(9223372036854775807, result1);
        }

        [Test]
        public void GetFloat_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] {
                0xc0, 0xc0, 0x00, 0x00,
                0x46, 0x10, 0x1d, 0x7d
            });

            float result0 = reader.GetFloat();
            float result1 = reader.GetFloat();

            Assert.AreEqual(-6, result0);
            Assert.AreEqual(9223.3720703125, result1);
        }

        [Test]
        public void GetDouble_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] {
                0xc0, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x40, 0xc2, 0x03, 0xaf, 0xa0, 0x00, 0x00, 0x00,
            });

            double result0 = reader.GetDouble();
            double result1 = reader.GetDouble();

            Assert.AreEqual(-6, result0);
            Assert.AreEqual(9223.3720703125, result1);
        }

        [Test]
        public void GetBytes_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] {
                0xc0, 0x18, 0x00, 0x00, 0x00, 0x04
            });

            byte[] result0 = reader.GetBytes(4);
            byte[] result1 = reader.GetBytes(2);

            Assert.AreEqual(new byte[] { 0xc0, 0x18, 0x00, 0x00 }, result0);
            Assert.AreEqual(new byte[] { 0x00, 0x04 }, result1);
        }

        [Test]
        public void GetVarInt_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] {
                0x00,
                0x01,
                0x7f,
                0x80, 0x01,
                0x80, 0x80, 0x80, 0x80, 0x08
            });

            VarInt result0 = reader.GetVarInt();
            VarInt result1 = reader.GetVarInt();
            VarInt result2 = reader.GetVarInt();
            VarInt result3 = reader.GetVarInt();
            VarInt result4 = reader.GetVarInt();

            Assert.AreEqual(0, result0.Value);
            Assert.AreEqual(1, result1.Value);
            Assert.AreEqual(127, result2.Value);
            Assert.AreEqual(128, result3.Value);
            Assert.AreEqual(-2147483648, result4.Value);
        }

        [Test]
        public void GetVarLong_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] {
                0x00,
                0x01,
                0x7f,
                0x80, 0x01,
                0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01
            });

            VarLong result0 = reader.GetVarLong();
            VarLong result1 = reader.GetVarLong();
            VarLong result2 = reader.GetVarLong();
            VarLong result3 = reader.GetVarLong();
            VarLong result4 = reader.GetVarLong();

            Assert.AreEqual(0, result0.Value);
            Assert.AreEqual(1, result1.Value);
            Assert.AreEqual(127, result2.Value);
            Assert.AreEqual(128, result3.Value);
            Assert.AreEqual(-9223372036854775808, result4.Value);
        }

        [Test]
        public void GetString_ValidData_ReturnsCorrectValue()
        {
            string str = "TÄMÄ on testi持燃ク禁購ヤホキ前追にゆびト見柳フ止図主のあ。鯉メヤロヒ連降タ誌番アノキロ氏難ア問士こ仰律ぽぱ韓新セチ動呼トは意触げ深学じふたい時Lorem ipsúm dolor sit amet, his cú habeo labítur, eos gloriatur omittantur ad, ex ius solet possim indoctum. Summo vólumus añ mel, sed ex doctus nostrúd, hás eu quis diám sóleat. Eúm at legeré ígnota conclusiónemque. Et meí suavitáte principes. Et sumo éverti quo, ex apeírian mnésarchum temporibus eam. Ad minim quidam sít, verí temporibus hás in.38制ろ問権タメ持掲各ぽーろこ避防覚創ひあ。会むっリ岡都むめびく徳処ミ命6置レの格討ゆ女空をりらに渡1年て予鰒ミツ意組ドるちぼ悪2企ノタヌオ辞4水ヱニヘユ積浩つわんち。";
            List<byte> bytes = new List<byte>();
            bytes.AddRange(new byte[] { 0b10110011, 0b00000110 });
            bytes.AddRange(Encoding.UTF8.GetBytes(str));
            bytes.AddRange(new byte[] { 0 });
            PacketReader reader = new PacketReader();

            reader.SetPacket(bytes.ToArray());

            string result0 = reader.GetString();
            string result1 = reader.GetString();

            Assert.AreEqual(str, result0);
            Assert.AreEqual("", result1);
        }

        [Test]
        public void GetChat_ValidData_ReturnsCorrectValue()
        {
            string str = "TÄMÄ on testi持燃ク禁購ヤホキ前追にゆびト見柳フ止図主のあ。鯉メヤロヒ連降タ誌番アノキロ氏難ア問士こ仰律ぽぱ韓新セチ動呼トは意触げ深学じふたい時Lorem ipsúm dolor sit amet, his cú habeo labítur, eos gloriatur omittantur ad, ex ius solet possim indoctum. Summo vólumus añ mel, sed ex doctus nostrúd, hás eu quis diám sóleat. Eúm at legeré ígnota conclusiónemque. Et meí suavitáte principes. Et sumo éverti quo, ex apeírian mnésarchum temporibus eam. Ad minim quidam sít, verí temporibus hás in.38制ろ問権タメ持掲各ぽーろこ避防覚創ひあ。会むっリ岡都むめびく徳処ミ命6置レの格討ゆ女空をりらに渡1年て予鰒ミツ意組ドるちぼ悪2企ノタヌオ辞4水ヱニヘユ積浩つわんち。";
            List<byte> bytes = new List<byte>();
            bytes.AddRange(new byte[] { 0b10110011, 0b00000110 });
            bytes.AddRange(Encoding.UTF8.GetBytes(str));
            bytes.AddRange(new byte[] { 0 });
            PacketReader reader = new PacketReader();

            reader.SetPacket(bytes.ToArray());

            Chat result0 = reader.GetChat();
            Chat result1 = reader.GetChat();

            Assert.AreEqual(str, result0.Value);
            Assert.AreEqual("", result1.Value);
        }

        [Test]
        public void GetPosition_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] {
                0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfd
            });

            Position result0 = reader.GetPosition();

            Assert.AreEqual(0b01111111111111111111111111, result0.X);
            Assert.AreEqual(0b111111111111, result0.Y);
            Assert.AreEqual(0b11111111111111111111111101, result0.Z);
        }

        [Test]
        public void GetAngle_ValidData_ReturnsCorrectValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] {
                0x8f
            });

            Angle result0 = reader.GetAngle();

            Assert.AreEqual(0x8f, result0.Value);
        }

        [Test]
        public void GetGuid_ValidData_ReturnsCorrectValue()
        {
            Guid expected = Guid.NewGuid();
            PacketReader reader = new PacketReader();

            reader.SetPacket(expected.ToByteArray()
                    .Reverse()
                    .ToArray());

            Guid result0 = reader.GetGuid();

            Assert.AreEqual(expected, result0);
        }

        [Test]
        public void ResetPosition_AfterReading_Works()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] { 1, 0 });
            reader.GetBool();

            reader.ResetPosition();

            bool result = reader.GetBool();

            Assert.AreEqual(true, result);
        }

        [Test]
        public void GetRemainingBytes_WithData_ReturnValidValue()
        {
            PacketReader reader = new PacketReader();

            reader.SetPacket(new byte[] { 1, 0 });
            reader.GetBool();
            int result = reader.GetRemainingBytes();

            Assert.AreEqual(1, result);
        }
    }
}
