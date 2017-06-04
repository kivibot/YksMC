using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Packets;
using YksMC.Protocol.Packets.Login;
using YksMC.Protocol.Packets.Play;
using YksMC.Protocol.Packets.Play.Clientbound;
using YksMC.Protocol.Packets.Status;

namespace YksMC.Client.Mapper
{
    public static class PacketTypeMapperExtensions
    {
        public static void RegisterVanillaPackets(this IPacketTypeMapper mapper)
        {
            Assembly assembly = typeof(HandshakePacket).GetTypeInfo().Assembly;
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.GetTypeInfo().GetCustomAttributes<PacketAttribute>().Any())
                    continue;

                mapper.RegisterType(type);
            }
        }
    }
}
