using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Protocol.Models.Packets.Login;

namespace YksMC.Clients.Handlers
{
    public class LoginHandler : IPacketHandler<DisconnectPacket>
    {
        public Task HandleAsync(DisconnectPacket packet)
        {
            throw new NotImplementedException();
        }
    }
}
