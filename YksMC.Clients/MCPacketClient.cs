using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YksMC.Protocol;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Clients
{
    public class MCPacketClient : IMCPacketClient
    {

        public MCPacketClient(IMCPacketReader packetReader, IMCPacketWriter packetWriter)
        {

        }

        public Task<T> ReceiveAsync<T>(CancellationToken cancelToken = default(CancellationToken)) where T : AbstractPacket
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(AbstractPacket packet, CancellationToken cancelToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
