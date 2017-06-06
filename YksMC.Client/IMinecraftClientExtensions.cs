using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;
using YksMC.Protocol.Packets;
using YksMC.Protocol.Packets.Login;
using YksMC.Protocol.Packets.Play.Common;

namespace YksMC.Client
{
    public static class IMinecraftClientExtensions
    {
        public static void SendHandshake(this IMinecraftClient client, ConnectionState nextState)
        {
            client.SendPacket(new HandshakePacket()
            {
                NextState = nextState,
                ProtocolVersion = client.ProtocolVersion,
                ServerAddress = client.Address.Host,
                ServerPort = client.Address.Port
            });
        }

        public static void SendLoginStartPacket(this IMinecraftClient client, string username)
        {
            client.SendPacket(new LoginStartPacket()
            {
                Name = username
            });
        }

        public static void SendKeepAlive(this IMinecraftClient client, VarInt keepAliveId)
        {
            client.SendPacket(new KeepAlivePacket()
            {
                KeepAliveId = keepAliveId
            });
        }
        
    }
}
