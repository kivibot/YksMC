using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;

namespace YksMC.Client.Mapper
{
    public class PacketTypeMapper : IPacketTypeMapper
    {
        private Dictionary<Tuple<ConnectionState, BoundTo, int>, Type> _types;
        private Dictionary<Tuple<BoundTo, Type>, int> _ids;

        public PacketTypeMapper()
        {
            _types = new Dictionary<Tuple<ConnectionState, BoundTo, int>, Type>();
            _ids = new Dictionary<Tuple<BoundTo, Type>, int>();
        }

        public int GetPacketId(BoundTo boundTo, Type type)
        {
            int id;
            if (!_ids.TryGetValue(new Tuple<BoundTo, Type>(boundTo, type), out id))
                throw new ArgumentException("Unknown packet: " + type);
            return id;
        }

        public Type GetPacketType(ConnectionState connectionState, BoundTo boundTo, int id)
        {
            Type type;
            if (!_types.TryGetValue(new Tuple<ConnectionState, BoundTo, int>(connectionState, boundTo, id), out type))
                return null;
            return type;
        }

        public void RegisterType(Type type)
        {
            IEnumerable<PacketAttribute> packetInfos = type.GetTypeInfo().GetCustomAttributes<PacketAttribute>();
            if (!packetInfos.Any())
                throw new ArgumentException($"Type {type} does not define a {nameof(PacketAttribute)}");
            foreach(PacketAttribute packetInfo in packetInfos)
                RegisterType(type, packetInfo.ConnectionState, packetInfo.BoundTo, packetInfo.Id);
        }

        private void RegisterType(Type type, ConnectionState connectionState, BoundTo boundTo, int id)
        {
            Tuple<ConnectionState, BoundTo, int> typeKey = new Tuple<ConnectionState, BoundTo, int>(connectionState, boundTo, id);
            if (_types.ContainsKey(typeKey))
                throw new ArgumentException($"A packet with the same attributes as '{type}' already exists: '{_types[typeKey]}'");

            _types.Add(typeKey, type);
            _ids.Add(new Tuple<BoundTo, Type>(boundTo, type), id);
        }
    }
}
