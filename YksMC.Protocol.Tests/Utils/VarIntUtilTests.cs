using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Utils;

namespace YksMC.Protocol.Tests.Utils
{
    [TestFixture]
    public class VarIntUtilTests
    {
        [Test]
        [TestCase(0, new byte[] { 0x00 })]
        [TestCase(1, new byte[] { 0x01 })]
        [TestCase(2, new byte[] { 0x02 })]
        [TestCase(127, new byte[] { 0x7f })]
        [TestCase(128, new byte[] { 0x80, 0x01 })]
        [TestCase(255, new byte[] { 0xff, 0x01 })]
        [TestCase(2147483647, new byte[] { 0xff, 0xff, 0xff, 0xff, 0x07 })]
        [TestCase(-1, new byte[] { 0xff, 0xff, 0xff, 0xff, 0x0f })]
        [TestCase(-2147483648, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x08 })]
        public void TestEncodeVarInt(int value, byte[] expected)
        {
            byte[] result = VarIntUtil.EncodeVarInt(value);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(0, new byte[] { 0x00 })]
        [TestCase(1, new byte[] { 0x01 })]
        [TestCase(2, new byte[] { 0x02 })]
        [TestCase(127, new byte[] { 0x7f })]
        [TestCase(128, new byte[] { 0x80, 0x01 })]
        [TestCase(255, new byte[] { 0xff, 0x01 })]
        [TestCase(-1, new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x01 })]
        [TestCase(9223372036854775807, new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x7f })]
        [TestCase(-9223372036854775808, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01 })]
        public void TestEncodeVarLong(long value, byte[] expected)
        {
            byte[] result = VarIntUtil.EncodeVarLong(value);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(0, new byte[] { 0x00 })]
        [TestCase(1, new byte[] { 0x01 })]
        [TestCase(2, new byte[] { 0x02 })]
        [TestCase(127, new byte[] { 0x7f })]
        [TestCase(128, new byte[] { 0x80, 0x01 })]
        [TestCase(255, new byte[] { 0xff, 0x01 })]
        [TestCase(2147483647, new byte[] { 0xff, 0xff, 0xff, 0xff, 0x07 })]
        [TestCase(-1, new byte[] { 0xff, 0xff, 0xff, 0xff, 0x0f })]
        [TestCase(-2147483648, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x08 })]
        public void TestDecodeVarInt(int expected, byte[] data)
        {
            int result = VarIntUtil.DecodeVarInt(data);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(0, new byte[] { 0x00 })]
        [TestCase(1, new byte[] { 0x01 })]
        [TestCase(2, new byte[] { 0x02 })]
        [TestCase(127, new byte[] { 0x7f })]
        [TestCase(128, new byte[] { 0x80, 0x01 })]
        [TestCase(255, new byte[] { 0xff, 0x01 })]
        [TestCase(-1, new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x01 })]
        [TestCase(9223372036854775807, new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x7f })]
        [TestCase(-9223372036854775808, new byte[] { 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x80, 0x01 })]
        public void TestDecodeVarLong(long expected, byte[] data)
        {
            long result = VarIntUtil.DecodeVarLong(data);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(0, new byte[] { 7, 1, 2, 0x00 }, 3)]
        [TestCase(1, new byte[] { 6, 2, 0x01 }, 2)]
        [TestCase(-1, new byte[] { 0xff, 0, 0xff, 0xff, 0xff, 0xff, 0x0f }, 2)]
        public void TestDecodeVarIntWithOffset(int expected, byte[] data, int offset)
        {
            int result = VarIntUtil.DecodeVarInt(data, offset);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase(0, new byte[] { 1, 0x00 }, 1)]
        [TestCase(1, new byte[] { 0xff, 0, 0x01 }, 2)]
        [TestCase(-1, new byte[] { 7, 7, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x01 }, 2)]
        public void TestDecodeVarLongWithOffset(long expected, byte[] data, int offset)
        {
            long result = VarIntUtil.DecodeVarLong(data, offset);
            Assert.AreEqual(expected, result);
        }
    }
}
