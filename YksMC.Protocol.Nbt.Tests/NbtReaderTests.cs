using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Tests.Fakes;
using YksMC.Protocol.Nbt;
using YksMC.Protocol.Nbt.Models;
using System.Linq;

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
            NbtReader reader = new NbtReader();

            ByteTag tag = reader.GetTag<ByteTag>(packetReader);

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
            NbtReader reader = new NbtReader();

            ShortTag tag = reader.GetTag<ShortTag>(packetReader);

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
            NbtReader reader = new NbtReader();

            IntTag tag = reader.GetTag<IntTag>(packetReader);

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
            NbtReader reader = new NbtReader();

            LongTag tag = reader.GetTag<LongTag>(packetReader);

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
            NbtReader reader = new NbtReader();

            FloatTag tag = reader.GetTag<FloatTag>(packetReader);

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
            NbtReader reader = new NbtReader();

            DoubleTag tag = reader.GetTag<DoubleTag>(packetReader);

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
            NbtReader reader = new NbtReader();

            ByteArrayTag tag = reader.GetTag<ByteArrayTag>(packetReader);

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
            NbtReader reader = new NbtReader();

            StringTag tag = reader.GetTag<StringTag>(packetReader);

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
            NbtReader reader = new NbtReader();

            CompoundTag tag = reader.GetTag<CompoundTag>(packetReader);

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
            NbtReader reader = new NbtReader();

            CompoundTag tag = reader.GetTag<CompoundTag>(packetReader);

            Assert.IsInstanceOf<CompoundTag>(tag.Tags[0]);
            Assert.IsInstanceOf<DoubleTag>(tag.Tags[1]);
        }

        [Test]
        public void ReadTag_EndOnly_ReturnsNull()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x00
            );
            NbtReader reader = new NbtReader();

            BaseTag tag = reader.GetTag<BaseTag>(packetReader);

            Assert.IsNull(tag);
        }

        [Test]
        public void ReadTag_InvalidTagType_Throws()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x55
            );
            NbtReader reader = new NbtReader();

            Assert.Throws<ArgumentException>(() =>
            {
                reader.GetTag<BaseTag>(packetReader);
            });
        }

        [Test]
        public void ReadIntArrayTag_ValidData_ReturnsCorrect()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x0B,
                (short)3,
                Encoding.UTF8.GetBytes("Moi"),
                4,
                1,
                2,
                3,
                1337
            );
            NbtReader reader = new NbtReader();

            IntArrayTag tag = reader.GetTag<IntArrayTag>(packetReader);

            Assert.AreEqual("Moi", tag.Name);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 1337 }, tag.Values);
        }

        [Test]
        public void ReadListTag_ValidData_ReturnsCorrect()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x09,
                (short)3,
                Encoding.UTF8.GetBytes("Moi"),
                (byte)0x01,
                3,
                (byte)4,
                (byte)1,
                (byte)2
            );
            NbtReader reader = new NbtReader();

            ListTag tag = reader.GetTag<ListTag>(packetReader);

            Assert.AreEqual("Moi", tag.Name);
            CollectionAssert.AreEqual(new byte[] { 4, 1, 2 }, tag.Values.Select(v => (ByteTag)v).Select(b => b.Value));
        }

        [Test]
        public void ReadListTag_InvalidType_Throws()
        {
            FakePacketReader packetReader = new FakePacketReader(
                (byte)0x09,
                (short)3,
                Encoding.UTF8.GetBytes("Moi"),
                (byte)0x80
            );
            NbtReader reader = new NbtReader();

            Assert.Throws<ArgumentException>(() =>
            {
                reader.GetTag<ListTag>(packetReader);
            });
        }
    }
}
