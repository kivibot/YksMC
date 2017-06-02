using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Packets;
using YksMC.Protocol.Models.Types;
using System.Reflection;

namespace YksMC.Protocol.Serializing
{
    public class PacketSerializer : IPacketSerializer
    {
        private readonly Dictionary<Type, Action<IPacketBuilder, object>> _propertyTypes;

        public PacketSerializer()
        {
            _propertyTypes = new Dictionary<Type, Action<IPacketBuilder, object>>();

            RegisterPropertyTypes();
        }

        public void Serialize(object packet, IPacketBuilder builder)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            Type type = packet.GetType();
            foreach (PropertyInfo property in type.GetRuntimeProperties())
            {
                Action<IPacketBuilder, object> serializeProperty;
                if (property.PropertyType.GetTypeInfo().IsEnum)
                    serializeProperty = SerializeEnum;
                else if (!_propertyTypes.TryGetValue(property.PropertyType, out serializeProperty))
                    throw new ArgumentException($"Unsupported property type: {property.PropertyType}");

                object value = property.GetValue(packet);
                if (value == null)
                    throw new ArgumentException($"Null property: {property.Name}");

                serializeProperty(builder, value);
            }
        }

        private void RegisterPropertyTypes()
        {
            RegisterPropertyType<bool>((b, v) => b.PutBool(v));
            RegisterPropertyType<sbyte>((b, v) => b.PutSignedByte(v));
            RegisterPropertyType<byte>((b, v) => b.PutByte(v));
            RegisterPropertyType<short>((b, v) => b.PutShort(v));
            RegisterPropertyType<ushort>((b, v) => b.PutUnsignedShort(v));
            RegisterPropertyType<int>((b, v) => b.PutInt(v));
            RegisterPropertyType<uint>((b, v) => b.PutUnsignedInt(v));
            RegisterPropertyType<long>((b, v) => b.PutLong(v));
            RegisterPropertyType<ulong>((b, v) => b.PutUnsignedLong(v));
            RegisterPropertyType<float>((b, v) => b.PutFloat(v));
            RegisterPropertyType<double>((b, v) => b.PutDouble(v));
            RegisterPropertyType<string>((b, v) => b.PutString(v));
            RegisterPropertyType<Chat>((b, v) => b.PutChat(v));
            RegisterPropertyType<VarInt>((b, v) => b.PutVarInt(v));
            RegisterPropertyType<VarLong>((b, v) => b.PutVarLong(v));
            RegisterPropertyType<Position>((b, v) => b.PutPosition(v));
            RegisterPropertyType<Angle>((b, v) => b.PutAngle(v));
            RegisterPropertyType<Guid>((b, v) => b.PutGuid(v));
            RegisterPropertyType<ByteArray>((b, v) => b.PutByteArray(v));
        }

        private void RegisterPropertyType<T>(Action<IPacketBuilder, T> func)
        {
            _propertyTypes[typeof(T)] = (r, v) => func(r, (T)v);
        }

        private void SerializeEnum(IPacketBuilder builder, object value)
        {
            builder.PutVarInt(new VarInt((int)value));
        }
    }
}
