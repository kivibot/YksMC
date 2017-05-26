using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YksMC.MCProtocol
{
    public class MCStatusClientBuilder : IMCStatusClientBuilder
    {
        private string _host;
        private ushort _port;

        public async Task<IMCStatusClient> BuildAsync(CancellationToken cancelToken = default(CancellationToken))
        {
            TcpClient tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(_host, _port);

            Stream stream = tcpClient.GetStream();

            return new MCStatusClient(new MCPacketReader(new StreamMCPacketSource(stream)), new MCPacketWriter(new StreamMCPacketSink(stream)));
        }

        public IMCClientBuilder UsingServer(string host, ushort port)
        {
            _host = host;
            _port = port;
            return this;
        }
    }
}
