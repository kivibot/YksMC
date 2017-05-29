using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YksMC.Protocol.Models.Exceptions;
using YksMC.Protocol.Tests.Fakes;
using YksMC.Protocol.Utils;

namespace YksMC.Protocol.Tests
{
    [TestFixture]
    public class StreamMCConnectionTests
    {
        [Test]
        public void TestDisposeDisposesTheUnderlyingStream()
        {
            FakeStream stream = new FakeStream();
            StreamMCConnection connection = new StreamMCConnection(stream);

            connection.Dispose();

            Assert.IsTrue(stream.IsDisposed);
        }

        [Test]
        public async Task TestSendPacketAsyncWithEmptyPacket()
        {
            MemoryStream stream = new MemoryStream();
            StreamMCConnection connection = new StreamMCConnection(stream);

            await connection.SendPacketAsync(new byte[0]);

            Assert.AreEqual(VarIntUtil.EncodeVarInt(0), stream.ToArray());
        }

        [Test]
        public async Task TestSendPacketAsyncSendsWholePacket()
        {
            MemoryStream stream = new MemoryStream();
            StreamMCConnection connection = new StreamMCConnection(stream);
            List<byte> data = new List<byte>();
            data.AddRange(new byte[500]);
            data.Add(123);

            await connection.SendPacketAsync(data.ToArray());

            byte[] lengthData = VarIntUtil.EncodeVarInt(data.Count);
            Assert.AreEqual(lengthData, stream.ToArray().Take(lengthData.Length));
            Assert.AreEqual(data.Count + lengthData.Length, stream.ToArray().Length);
            Assert.AreEqual(123, stream.ToArray().Last());
        }

        [Test]
        public void TestConstructorThrowsOnNullStream()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new StreamMCConnection(null);
            });
        }

        [Test]
        public async Task TestReceivePacketAsyncReturnsNullOnValidEot()
        {
            MemoryStream stream = new MemoryStream();
            StreamMCConnection connection = new StreamMCConnection(stream);

            byte[] result = await connection.ReceivePacketAsync();

            Assert.IsNull(result);
        }

        [Test]
        public void TestReceivePacketAsyncThrowsOnInvalidEotInData()
        {
            List<byte> data = new List<byte>();
            data.AddRange(VarIntUtil.EncodeVarInt(255));
            MemoryStream stream = new MemoryStream(data.ToArray());
            StreamMCConnection connection = new StreamMCConnection(stream);

            Assert.ThrowsAsync<PacketSourceException>(async () =>
            {
                await connection.ReceivePacketAsync();
            });
        }

        [Test]
        public void TestReceivePacketAsyncThrowsOnInvalidVarInt()
        {
            List<byte> data = new List<byte>();
            data.AddRange(VarIntUtil.EncodeVarLong(long.MaxValue));
            MemoryStream stream = new MemoryStream(data.ToArray());
            StreamMCConnection connection = new StreamMCConnection(stream);

            Assert.ThrowsAsync<PacketSourceException>(async () =>
            {
                await connection.ReceivePacketAsync();
            });
        }

        [Test]
        public void TestReceivePacketAsyncThrowsOnInvalidEotInLenght()
        {
            List<byte> data = new List<byte>();
            data.Add(0xff);
            MemoryStream stream = new MemoryStream(data.ToArray());
            StreamMCConnection connection = new StreamMCConnection(stream);

            Assert.ThrowsAsync<PacketSourceException>(async () =>
            {
                await connection.ReceivePacketAsync();
            });
        }

        [Test]
        public async Task TestReceivePacketAsyncReadsWholePacket()
        {
            List<byte> data = new List<byte>();
            data.AddRange(VarIntUtil.EncodeVarInt(255));
            data.AddRange(new byte[254]);
            data.Add(6);
            MemoryStream stream = new MemoryStream(data.ToArray());
            StreamMCConnection connection = new StreamMCConnection(stream);

            byte[] result = await connection.ReceivePacketAsync();

            Assert.AreEqual(255, result.Length);
            Assert.AreEqual(6, result[result.Length - 1]);
        }

        [Test]
        public async Task TestReceivePacketAsyncCanReadEmptyPackets()
        {
            List<byte> data = new List<byte>();
            data.AddRange(VarIntUtil.EncodeVarInt(0));
            MemoryStream stream = new MemoryStream(data.ToArray());
            StreamMCConnection connection = new StreamMCConnection(stream);

            byte[] result = await connection.ReceivePacketAsync();

            Assert.AreEqual(0, result.Length);
        }
    }
}
