using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YksMC.Protocol.Tests.Fakes;
using YksMC.Protocol;
using YksMC.Protocol.Models.Exceptions;
using YksMC.Protocol.Utils;

namespace YksMC.Protocol.Tests
{
    [TestFixture]
    public class StreamMCPacketSourceTests
    {

        private StreamMCPacketSource _source;

        [TearDown]
        public void TearDown()
        {
            if (_source != null)
                _source.Dispose();
        }

        [Test]
        public void TestDisposeDisposesTheUnderlyingStream()
        {
            FakeStream stream = new FakeStream();
            _source = new StreamMCPacketSource(stream);

            _source.Dispose();

            Assert.IsTrue(stream.IsDisposed);
        }

        [Test]
        public void TestConstructorThrowsOnNullStream()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new StreamMCPacketSource(null);
            });
        }

        [Test]
        public async Task TestGetNextAsyncReturnsNullOnValidEot()
        {
            MemoryStream stream = new MemoryStream();
            _source = new StreamMCPacketSource(stream);

            byte[] result = await _source.GetNextAsync();

            Assert.IsNull(result);
        }

        [Test]
        public void TestGetNextAsyncThrowsOnInvalidEotInData()
        {
            List<byte> data = new List<byte>();
            data.AddRange(VarIntUtil.EncodeVarInt(255));
            MemoryStream stream = new MemoryStream(data.ToArray());
            _source = new StreamMCPacketSource(stream);

            Assert.ThrowsAsync<PacketSourceException>(async () =>
            {
                await _source.GetNextAsync();
            });
        }

        [Test]
        public void TestGetNextAsyncThrowsOnInvalidVarInt()
        {
            List<byte> data = new List<byte>();
            data.AddRange(VarIntUtil.EncodeVarLong(long.MaxValue));
            MemoryStream stream = new MemoryStream(data.ToArray());
            _source = new StreamMCPacketSource(stream);

            Assert.ThrowsAsync<PacketSourceException>(async () =>
            {
                await _source.GetNextAsync();
            });
        }

        [Test]
        public void TestGetNextAsyncThrowsOnInvalidEotInLenght()
        {
            List<byte> data = new List<byte>();
            data.Add(0xff);
            MemoryStream stream = new MemoryStream(data.ToArray());
            _source = new StreamMCPacketSource(stream);

            Assert.ThrowsAsync<PacketSourceException>(async () =>
            {
                await _source.GetNextAsync();
            });
        }

        [Test]
        public async Task TestGetNextAsyncReadsWholePacket()
        {
            List<byte> data = new List<byte>();
            data.AddRange(VarIntUtil.EncodeVarInt(255));
            data.AddRange(new byte[254]);
            data.Add(6);
            MemoryStream stream = new MemoryStream(data.ToArray());
            _source = new StreamMCPacketSource(stream);

            byte[] result = await _source.GetNextAsync();

            Assert.AreEqual(255, result.Length);
            Assert.AreEqual(6, result[result.Length - 1]);
        }

        [Test]
        public async Task TestGetNextAsyncCanReadEmptyPackets()
        {
            List<byte> data = new List<byte>();
            data.AddRange(VarIntUtil.EncodeVarInt(0));
            MemoryStream stream = new MemoryStream(data.ToArray());
            _source = new StreamMCPacketSource(stream);

            byte[] result = await _source.GetNextAsync();

            Assert.AreEqual(0, result.Length);
        }
    }
}
