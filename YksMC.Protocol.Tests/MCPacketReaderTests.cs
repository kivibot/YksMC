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

namespace YksMC.Protocol.Tests
{
    [TestFixture]
    public class MCPacketReaderTests
    {
        [Test]
        public async Task TestNextAsyncReturnValues()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[1], new byte[1]);
            MCPacketReader reader = new MCPacketReader(connection);

            bool result0 = await reader.NextAsync();
            bool result1 = await reader.NextAsync();
            bool result2 = await reader.NextAsync();

            Assert.IsTrue(result0);
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        [Test]
        public async Task TestNextAsyncResetsPosition()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] { 1 }, new byte[] { 3 });
            MCPacketReader reader = new MCPacketReader(connection);

            bool result0 = await reader.NextAsync();
            byte result1 = reader.GetByte();
            bool result2 = await reader.NextAsync();
            byte result3 = reader.GetByte();

            Assert.IsTrue(result0);
            Assert.AreEqual(1, result1);
            Assert.IsTrue(result2);
            Assert.AreEqual(3, result3);
        }

        [Test]
        public async Task TestGetBool()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] { 0, 1 });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            bool result0 = reader.GetBool();
            bool result1 = reader.GetBool();

            Assert.AreEqual(false, result0);
            Assert.AreEqual(true, result1);
        }

        [Test]
        public async Task TestGetSignedByte()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] { 1, 250 });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            sbyte result0 = reader.GetSignedByte();
            sbyte result1 = reader.GetSignedByte();

            Assert.AreEqual(1, result0);
            Assert.AreEqual(-6, result1);
        }

        [Test]
        public async Task TestGetByte()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] { 1, 250 });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            byte result0 = reader.GetByte();
            byte result1 = reader.GetByte();

            Assert.AreEqual(1, result0);
            Assert.AreEqual(250, result1);
        }

        [Test]
        public async Task TestGetShort()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] { 0x80, 0x01, 0x7f, 0xff });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            short result0 = reader.GetShort();
            short result1 = reader.GetShort();

            Assert.AreEqual(-32767, result0);
            Assert.AreEqual(32767, result1);
        }

        [Test]
        public async Task TestGetUnsignedShort()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] { 0x00, 0x03, 0xa7, 0x0f });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            ushort result0 = reader.GetUnsignedShort();
            ushort result1 = reader.GetUnsignedShort();

            Assert.AreEqual(3, result0);
            Assert.AreEqual(42767, result1);
        }

        [Test]
        public async Task TestGetInt()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] { 0xff, 0xff, 0xff, 0xfb, 0x7f, 0xff, 0xff, 0xff });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            int result0 = reader.GetInt();
            int result1 = reader.GetInt();

            Assert.AreEqual(-5, result0);
            Assert.AreEqual(2147483647, result1);
        }

        [Test]
        public async Task TestGetUnsignedInt()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] { 0xff, 0xff, 0xff, 0xfb, 0x7f, 0xff, 0xff, 0xff });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            uint result0 = reader.GetUnsignedInt();
            uint result1 = reader.GetUnsignedInt();

            Assert.AreEqual(4294967291, result0);
            Assert.AreEqual(2147483647, result1);
        }

        [Test]
        public async Task TestGetLong()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] {
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfb,
                0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
            });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            long result0 = reader.GetLong();
            long result1 = reader.GetLong();

            Assert.AreEqual(-5, result0);
            Assert.AreEqual(9223372036854775807, result1);
        }

        [Test]
        public async Task TestGetUnsignedLong()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] {
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfb,
                0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
            });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            ulong result0 = reader.GetUnsignedLong();
            ulong result1 = reader.GetUnsignedLong();

            Assert.AreEqual(ulong.MaxValue - 4, result0);
            Assert.AreEqual(9223372036854775807, result1);
        }

        [Test]
        public async Task TestGetFloat()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] {
                0xc0, 0xc0, 0x00, 0x00,
                0x46, 0x10, 0x1d, 0x7d
            });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            float result0 = reader.GetFloat();
            float result1 = reader.GetFloat();

            Assert.AreEqual(-6, result0);
            Assert.AreEqual(9223.3720703125, result1);
        }

        [Test]
        public async Task TestGetDouble()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] {
                0xc0, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x40, 0xc2, 0x03, 0xaf, 0xa0, 0x00, 0x00, 0x00,
            });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            double result0 = reader.GetDouble();
            double result1 = reader.GetDouble();

            Assert.AreEqual(-6, result0);
            Assert.AreEqual(9223.3720703125, result1);
        }

        [Test]
        public async Task TestGetBytes()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] {
                0xc0, 0x18, 0x00, 0x00, 0x00, 0x04
            });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            byte[] result0 = reader.GetBytes(4);
            byte[] result1 = reader.GetBytes(2);

            Assert.AreEqual(new byte[] { 0xc0, 0x18, 0x00, 0x00 }, result0);
            Assert.AreEqual(new byte[] { 0x00, 0x04 }, result1);
        }

        [Test]
        public async Task TestGetVarInt()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] {
                0x00,
                0x01,
                0x7f,
                0x80, 0x01,
                0x80, 0x80, 0x80, 0x80, 0x08
            });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

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
        public async Task TestGetVarLong()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] {
                0x00,
                0x01,
                0x7f,
                0x80, 0x01,
                0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01
            });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

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
        public async Task TestGetString()
        {
            string str = "TÄMÄ on testi持燃ク禁購ヤホキ前追にゆびト見柳フ止図主のあ。鯉メヤロヒ連降タ誌番アノキロ氏難ア問士こ仰律ぽぱ韓新セチ動呼トは意触げ深学じふたい時Lorem ipsúm dolor sit amet, his cú habeo labítur, eos gloriatur omittantur ad, ex ius solet possim indoctum. Summo vólumus añ mel, sed ex doctus nostrúd, hás eu quis diám sóleat. Eúm at legeré ígnota conclusiónemque. Et meí suavitáte principes. Et sumo éverti quo, ex apeírian mnésarchum temporibus eam. Ad minim quidam sít, verí temporibus hás in.38制ろ問権タメ持掲各ぽーろこ避防覚創ひあ。会むっリ岡都むめびく徳処ミ命6置レの格討ゆ女空をりらに渡1年て予鰒ミツ意組ドるちぼ悪2企ノタヌオ辞4水ヱニヘユ積浩つわんち。";
            List<byte> bytes = new List<byte>();
            bytes.AddRange(new byte[] { 0b10110011, 0b00000110 });
            bytes.AddRange(Encoding.UTF8.GetBytes(str));
            bytes.AddRange(new byte[] { 0 });
            FakeMCConnection connection = new FakeMCConnection(bytes.ToArray());
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            string result0 = reader.GetString();
            string result1 = reader.GetString();

            Assert.AreEqual(str, result0);
            Assert.AreEqual("", result1);
        }

        [Test]
        public async Task TestGetChat()
        {
            string str = "TÄMÄ on testi持燃ク禁購ヤホキ前追にゆびト見柳フ止図主のあ。鯉メヤロヒ連降タ誌番アノキロ氏難ア問士こ仰律ぽぱ韓新セチ動呼トは意触げ深学じふたい時Lorem ipsúm dolor sit amet, his cú habeo labítur, eos gloriatur omittantur ad, ex ius solet possim indoctum. Summo vólumus añ mel, sed ex doctus nostrúd, hás eu quis diám sóleat. Eúm at legeré ígnota conclusiónemque. Et meí suavitáte principes. Et sumo éverti quo, ex apeírian mnésarchum temporibus eam. Ad minim quidam sít, verí temporibus hás in.38制ろ問権タメ持掲各ぽーろこ避防覚創ひあ。会むっリ岡都むめびく徳処ミ命6置レの格討ゆ女空をりらに渡1年て予鰒ミツ意組ドるちぼ悪2企ノタヌオ辞4水ヱニヘユ積浩つわんち。";
            List<byte> bytes = new List<byte>();
            bytes.AddRange(new byte[] { 0b10110011, 0b00000110 });
            bytes.AddRange(Encoding.UTF8.GetBytes(str));
            bytes.AddRange(new byte[] { 0 });
            FakeMCConnection connection = new FakeMCConnection(bytes.ToArray());
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            Chat result0 = reader.GetChat();
            Chat result1 = reader.GetChat();

            Assert.AreEqual(str, result0.Value);
            Assert.AreEqual("", result1.Value);
        }

        [Test]
        public async Task TestGetPosition()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] {
                0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfd
            });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            Position result0 = reader.GetPosition();

            Assert.AreEqual(0b01111111111111111111111111, result0.X);
            Assert.AreEqual(0b111111111111, result0.Y);
            Assert.AreEqual(0b11111111111111111111111101, result0.Z);
        }

        [Test]
        public async Task TestGetAngle()
        {
            FakeMCConnection connection = new FakeMCConnection(new byte[] {
                0x8f
            });
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            Angle result0 = reader.GetAngle();

            Assert.AreEqual(0x8f, result0.Value);
        }

        [Test]
        public async Task TestGetGuid()
        {
            Guid expected = Guid.NewGuid();
            FakeMCConnection connection = new FakeMCConnection(
                expected.ToByteArray()
                    .Reverse()
                    .ToArray()
            );
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();

            Guid result0 = reader.GetGuid();

            Assert.AreEqual(expected, result0);
        }

        [Test]
        public async Task ResetPosition_AfterReading_Works()
        {
            FakeMCConnection connection = new FakeMCConnection(
                new byte[] { 1, 0 }
            );
            MCPacketReader reader = new MCPacketReader(connection);

            await reader.NextAsync();
            reader.GetBool();

            reader.ResetPosition();

            bool result = reader.GetBool();

            Assert.AreEqual(true, result);
        }
    }
}
