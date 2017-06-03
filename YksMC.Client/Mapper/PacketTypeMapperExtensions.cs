using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Packets;
using YksMC.Protocol.Packets.Login;
using YksMC.Protocol.Packets.Play;
using YksMC.Protocol.Packets.Status;

namespace YksMC.Client.Mapper
{
    public static class PacketTypeMapperExtensions
    {
        public static void RegisterVanillaPackets(this IPacketTypeMapper mapper)
        {
            //ConnectionState.Handshake
            mapper.RegisterType<HandshakePacket>();

            //ConnectionState.Status
            mapper.RegisterType<PingPacket>();
            mapper.RegisterType<PongPacket>();
            mapper.RegisterType<StatusRequestPacket>();
            mapper.RegisterType<StatusResponsePacket>();

            //ConnectionState.Login
            mapper.RegisterType<DisconnectPacket>();
            mapper.RegisterType<EncryptionResponsePacket>();
            mapper.RegisterType<EncryptionRequestPacket>();
            mapper.RegisterType<LoginStartPacket>();
            mapper.RegisterType<LoginSuccessPacket>();
            mapper.RegisterType<SetCompressionPacket>();

            //ConnectionState.Play
            mapper.RegisterType<JoinGamePacket>();
            mapper.RegisterType<PluginMessagePacket>();
        }
    }
}
