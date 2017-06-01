using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Packets;

namespace YksMC.Clients.Mapper
{
    public class PacketTypeMapper : IPacketTypeMapper
    {
        private Dictionary<Tuple<ConnectionState, BoundTo, int>, Type> _types;

        public PacketTypeMapper()
        {
            _types = new Dictionary<Tuple<ConnectionState, BoundTo, int>, Type>();
        }

        public Type GetPacketType(ConnectionState connectionState, BoundTo boundTo, int id)
        {
            Type type;
            if (!_types.TryGetValue(new Tuple<ConnectionState, BoundTo, int>(connectionState, boundTo, id), out type))
                return null;
            return type;
        }

        public void RegisterType<T>() where T : IPacket
        {
            Type type = typeof(T);
            PacketAttribute packetInfo = type.GetTypeInfo().GetCustomAttribute<PacketAttribute>();
            if (packetInfo == null)
                throw new ArgumentException($"Type {type} does not define a {nameof(PacketAttribute)}");
            RegisterType(type, packetInfo.ConnectionState, packetInfo.BoundTo, packetInfo.Id);
        }

        private void RegisterType(Type type, ConnectionState connectionState, BoundTo boundTo, int id)
        {
            Tuple<ConnectionState, BoundTo, int> key = new Tuple<ConnectionState, BoundTo, int>(connectionState, boundTo, id);
            if (_types.ContainsKey(key))
                throw new ArgumentException($"A packet with the same attributes as {type} already exists: {_types[key]}");
            _types.Add(key, type);
        }
    }
}
