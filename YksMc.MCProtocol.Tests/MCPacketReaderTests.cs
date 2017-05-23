using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMc.MCProtocol.Tests.Fakes;
using YksMC.MCProtocol;

namespace YksMc.MCProtocol.Tests
{
    [TestFixture]
    public class MCPacketReaderTests
    {
        [Test]
        public async Task TestNextAsyncReturnValues()
        {
            FakeMCPacketSource source = new FakeMCPacketSource(new byte[1], new byte[1]);
            MCPacketReader reader = new MCPacketReader(source);

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
            FakeMCPacketSource source = new FakeMCPacketSource(new byte[] { 1 }, new byte[] { 3 });
            MCPacketReader reader = new MCPacketReader(source);

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
            FakeMCPacketSource source = new FakeMCPacketSource(new byte[] { 0, 1 });
            MCPacketReader reader = new MCPacketReader(source);

            await reader.NextAsync();

            bool result0 = reader.GetBool();
            bool result1 = reader.GetBool();

            Assert.AreEqual(false, result0);
            Assert.AreEqual(true, result1);
        }

        [Test]
        public async Task TestGetSignedByte()
        {
            FakeMCPacketSource source = new FakeMCPacketSource(new byte[] { 1, 250 });
            MCPacketReader reader = new MCPacketReader(source);

            await reader.NextAsync();

            sbyte result0 = reader.GetSignedByte();
            sbyte result1 = reader.GetSignedByte();

            Assert.AreEqual(1, result0);
            Assert.AreEqual(-6, result1);
        }

        [Test]
        public async Task TestGetByte()
        {
            FakeMCPacketSource source = new FakeMCPacketSource(new byte[] { 1, 250 });
            MCPacketReader reader = new MCPacketReader(source);

            await reader.NextAsync();

            byte result0 = reader.GetByte();
            byte result1 = reader.GetByte();

            Assert.AreEqual(1, result0);
            Assert.AreEqual(250, result1);
        }

        [Test]
        public async Task TestGetShort()
        {
            FakeMCPacketSource source = new FakeMCPacketSource(new byte[] { 0x80, 0x01, 0x7f, 0xff });
            MCPacketReader reader = new MCPacketReader(source);

            await reader.NextAsync();

            short result0 = reader.GetShort();
            short result1 = reader.GetShort();

            Assert.AreEqual(-32767, result0);
            Assert.AreEqual(32767, result1);
        }

        [Test]
        public async Task TestGetUnsignedShort()
        {
            FakeMCPacketSource source = new FakeMCPacketSource(new byte[] { 0x00, 0x03, 0xa7, 0x0f });
            MCPacketReader reader = new MCPacketReader(source);

            await reader.NextAsync();

            ushort result0 = reader.GetUnsignedShort();
            ushort result1 = reader.GetUnsignedShort();

            Assert.AreEqual(3, result0);
            Assert.AreEqual(42767, result1);
        }

        [Test]
        public async Task TestGetInt()
        {
            FakeMCPacketSource source = new FakeMCPacketSource(new byte[] { 0xff, 0xff, 0xff, 0xfb, 0x7f, 0xff, 0xff, 0xff });
            MCPacketReader reader = new MCPacketReader(source);

            await reader.NextAsync();

            int result0 = reader.GetInt();
            int result1 = reader.GetInt();

            Assert.AreEqual(-5, result0);
            Assert.AreEqual(2147483647, result1);
        }

        [Test]
        public async Task TestGetUnsignedInt()
        {
            FakeMCPacketSource source = new FakeMCPacketSource(new byte[] { 0xff, 0xff, 0xff, 0xfb, 0x7f, 0xff, 0xff, 0xff });
            MCPacketReader reader = new MCPacketReader(source);

            await reader.NextAsync();

            uint result0 = reader.GetUnsignedInt();
            uint result1 = reader.GetUnsignedInt();

            Assert.AreEqual(4294967291, result0);
            Assert.AreEqual(2147483647, result1);
        }

        [Test]
        public async Task TestGetLong()
        {
            FakeMCPacketSource source = new FakeMCPacketSource(new byte[] {
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfb,
                0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
            });
            MCPacketReader reader = new MCPacketReader(source);

            await reader.NextAsync();

            long result0 = reader.GetLong();
            long result1 = reader.GetLong();

            Assert.AreEqual(-5, result0);
            Assert.AreEqual(9223372036854775807, result1);
        }

        [Test]
        public async Task TestGetUnsignedLong()
        {
            FakeMCPacketSource source = new FakeMCPacketSource(new byte[] {
                0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xfb,
                0x7f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
            });
            MCPacketReader reader = new MCPacketReader(source);

            await reader.NextAsync();

            ulong result0 = reader.GetUnsignedLong();
            ulong result1 = reader.GetUnsignedLong();

            Assert.AreEqual(ulong.MaxValue - 4, result0);
            Assert.AreEqual(9223372036854775807, result1);
        }

        [Test]
        public async Task TestGetFloat()
        {
            FakeMCPacketSource source = new FakeMCPacketSource(new byte[] {
                0xc0, 0xc0, 0x00, 0x00,
                0x46, 0x10, 0x1d, 0x7d
            });
            MCPacketReader reader = new MCPacketReader(source);

            await reader.NextAsync();

            float result0 = reader.GetFloat();
            float result1 = reader.GetFloat();

            Assert.AreEqual(-6, result0);
            Assert.AreEqual(9223.3720703125, result1);
        }

        [Test]
        public async Task TestGetDouble()
        {
            FakeMCPacketSource source = new FakeMCPacketSource(new byte[] {
                0xc0, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x40, 0xc2, 0x03, 0xaf, 0xa0, 0x00, 0x00, 0x00,
            });
            MCPacketReader reader = new MCPacketReader(source);

            await reader.NextAsync();

            double result0 = reader.GetDouble();
            double result1 = reader.GetDouble();

            Assert.AreEqual(-6, result0);
            Assert.AreEqual(9223.3720703125, result1);
        }

        [Test]
        public async Task TestGetBytes()
        {
            FakeMCPacketSource source = new FakeMCPacketSource(new byte[] {
                0xc0, 0x18, 0x00, 0x00, 0x00, 0x04
            });
            MCPacketReader reader = new MCPacketReader(source);

            await reader.NextAsync();

            byte[] result0 = reader.GetBytes(4);
            byte[] result1 = reader.GetBytes(2);

            Assert.AreEqual(new byte[] { 0xc0, 0x18, 0x00, 0x00 }, result0);
            Assert.AreEqual(new byte[] { 0x00, 0x04 }, result1);
        }
    }
}
