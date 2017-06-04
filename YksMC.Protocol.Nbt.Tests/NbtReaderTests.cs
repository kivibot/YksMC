using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Tests.Fakes;
using YksMC.Protocol.Nbt;
using YksMC.Protocol.Nbt.Models;

namespace YksMC.Protocol.Nbt.Tests
{
    [TestFixture]
    public class NbtReaderTests
    {
        [Test]
        public void ReadByteTag_ValidData_ReturnsCorrect()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x01,
                (short)5,
                Encoding.UTF8.GetBytes("Hello"),
                (byte)0x55
            );
            NbtReader reader = new NbtReader(packetReader);

            ByteTag tag = reader.GetTag<ByteTag>();

            Assert.AreEqual("Hello", tag.Name);
            Assert.AreEqual(0x55, tag.Value);
        }

        [Test]
        public void ReadShortTag_ValidData_ReturnsCorrect()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x02,
                (short)3,
                Encoding.UTF8.GetBytes("Moi"),
                (short)123
            );
            NbtReader reader = new NbtReader(packetReader);

            ShortTag tag = reader.GetTag<ShortTag>();

            Assert.AreEqual("Moi", tag.Name);
            Assert.AreEqual(123, tag.Value);
        }

        [Test]
        public void ReadIntTag_ValidData_ReturnsCorrect()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x03,
                (short)3,
                Encoding.UTF8.GetBytes("Moi"),
                1337
            );
            NbtReader reader = new NbtReader(packetReader);

            IntTag tag = reader.GetTag<IntTag>();

            Assert.AreEqual("Moi", tag.Name);
            Assert.AreEqual(1337, tag.Value);
        }

        [Test]
        public void ReadLongTag_ValidData_ReturnsCorrect()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x04,
                (short)3,
                Encoding.UTF8.GetBytes("Moi"),
                123L
            );
            NbtReader reader = new NbtReader(packetReader);

            LongTag tag = reader.GetTag<LongTag>();

            Assert.AreEqual("Moi", tag.Name);
            Assert.AreEqual(123, tag.Value);
        }

        [Test]
        public void ReadFloatTag_ValidData_ReturnsCorrect()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x05,
                (short)3,
                Encoding.UTF8.GetBytes("Moi"),
                123.5f
            );
            NbtReader reader = new NbtReader(packetReader);

            FloatTag tag = reader.GetTag<FloatTag>();

            Assert.AreEqual("Moi", tag.Name);
            Assert.AreEqual(123.5f, tag.Value);
        }

        [Test]
        public void ReadDoubleTag_ValidData_ReturnsCorrect()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x06,
                (short)3,
                Encoding.UTF8.GetBytes("Moi"),
                123.6
            );
            NbtReader reader = new NbtReader(packetReader);

            DoubleTag tag = reader.GetTag<DoubleTag>();

            Assert.AreEqual("Moi", tag.Name);
            Assert.AreEqual(123.6, tag.Value);
        }

        [Test]
        public void ReadByteArrayTag_ValidData_ReturnsCorrect()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x07,
                (short)3,
                Encoding.UTF8.GetBytes("Moi"),
                5,
                new byte[5]
            );
            NbtReader reader = new NbtReader(packetReader);

            ByteArrayTag tag = reader.GetTag<ByteArrayTag>();

            Assert.AreEqual("Moi", tag.Name);
            Assert.AreEqual(new byte[5], tag.Value);
        }

        [Test]
        public void ReadStringTag_ValidData_ReturnsCorrect()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x08,
                (short)3,
                Encoding.UTF8.GetBytes("Moi"),
                (short)3,
                Encoding.UTF8.GetBytes("Moi")
            );
            NbtReader reader = new NbtReader(packetReader);

            StringTag tag = reader.GetTag<StringTag>();

            Assert.AreEqual("Moi", tag.Name);
            Assert.AreEqual("Moi", tag.Value);
        }

        [Test]
        public void ReadCompoundTag_Empty_ReturnsCorrect()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x0A,
                (short)3,
                Encoding.UTF8.GetBytes("Moi"),
                (byte)0x00
            );
            NbtReader reader = new NbtReader(packetReader);

            CompoundTag tag = reader.GetTag<CompoundTag>();

            Assert.AreEqual("Moi", tag.Name);
            CollectionAssert.IsEmpty(tag.Tags);
        }

        [Test]
        public void ReadCompoundTag_TwoTags_ReturnsCorrect()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x0A,
                (short)3,
                Encoding.UTF8.GetBytes("Moi"),
                    (byte)0x0A,
                    (short)3,
                    Encoding.UTF8.GetBytes("Moi"),
                    (byte)0x00,

                    (byte)0x06,
                    (short)3,
                    Encoding.UTF8.GetBytes("Moi"),
                    123.6,
                (byte)0x00
            );
            NbtReader reader = new NbtReader(packetReader);

            CompoundTag tag = reader.GetTag<CompoundTag>();

            Assert.IsInstanceOf<CompoundTag>(tag.Tags[0]);
            Assert.IsInstanceOf<DoubleTag>(tag.Tags[1]);
        }

        [Test]
        public void ReadTag_EndOnly_ReturnsNull()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x00
            );
            NbtReader reader = new NbtReader(packetReader);

            BaseTag tag = reader.GetTag<BaseTag>();

            Assert.IsNull(tag);
        }

        [Test]
        public void ReadTag_InvalidTagType_Throws()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x55
            );
            NbtReader reader = new NbtReader(packetReader);

            Assert.Throws<ArgumentException>(() =>
            {
                reader.GetTag<BaseTag>();
            });
        }
    }
}
