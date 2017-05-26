using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YksMc.Protocol.Tests.Fakes;
using YksMC.MCProtocol;
using YksMC.MCProtocol.Utils;

namespace YksMc.Protocol.Tests
{
    [TestFixture]
    public class StreamMCPacketSinkTests
    {

        [Test]
        public void TestDisposeDisposesTheUnderlyingStream()
        {
            FakeStream stream = new FakeStream();
            StreamMCPacketSink sink = new StreamMCPacketSink(stream);

            sink.Dispose();

            Assert.IsTrue(stream.IsDisposed);
        }

        [Test]
        public async Task TestSendPacketAsyncWithEmptyPacket()
        {
            MemoryStream stream = new MemoryStream();
            StreamMCPacketSink sink = new StreamMCPacketSink(stream);

            await sink.SendPacketAsync(new byte[0]);

            Assert.AreEqual(VarIntUtil.EncodeVarInt(0), stream.ToArray());
        }

        [Test]
        public async Task TestSendPacketAsyncSendsWholePacket()
        {
            MemoryStream stream = new MemoryStream();
            StreamMCPacketSink sink = new StreamMCPacketSink(stream);
            List<byte> data = new List<byte>();
            data.AddRange(new byte[500]);
            data.Add(123);

            await sink.SendPacketAsync(data.ToArray());

            byte[] lengthData = VarIntUtil.EncodeVarInt(data.Count);
            Assert.AreEqual(lengthData, stream.ToArray().Take(lengthData.Length));
            Assert.AreEqual(data.Count + lengthData.Length, stream.ToArray().Length);
            Assert.AreEqual(123, stream.ToArray().Last());
        }

    }
}
