using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;
using System.Reflection;

namespace YksMC.Protocol.Serializing
{
    public class PacketSerializer : IPacketSerializer
    {
        private readonly Dictionary<Type, Action<object, IPacketBuilder>> _propertyTypes;

        public PacketSerializer()
        {
            _propertyTypes = new Dictionary<Type, Action<object, IPacketBuilder>>();

            RegisterPropertyTypes();
        }

        public void Serialize(object packet, IPacketBuilder builder)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            Type type = packet.GetType();
            foreach (PropertyInfo property in type.GetRuntimeProperties())
            {
                Action<object, IPacketBuilder> serializeProperty;
                if (!_propertyTypes.TryGetValue(property.PropertyType, out serializeProperty))
                {
                    if (property.PropertyType.GetTypeInfo().IsEnum)
                        serializeProperty = SerializeEnum;
                    else if (property.PropertyType.GetTypeInfo().IsClass)
                        serializeProperty = Serialize;
                    else
                        throw new ArgumentException($"Unsupported property type: {property.PropertyType}");
                }

                object value = property.GetValue(packet);
                if (value == null)
                    throw new ArgumentException($"Null property: {property.Name}");

                serializeProperty(value, builder);
            }
        }

        private void RegisterPropertyTypes()
        {
            RegisterPropertyType<bool>((v, b) => b.PutBool(v));
            RegisterPropertyType<sbyte>((v, b) => b.PutSignedByte(v));
            RegisterPropertyType<byte>((v, b) => b.PutByte(v));
            RegisterPropertyType<short>((v, b) => b.PutShort(v));
            RegisterPropertyType<ushort>((v, b) => b.PutUnsignedShort(v));
            RegisterPropertyType<int>((v, b) => b.PutInt(v));
            RegisterPropertyType<uint>((v, b) => b.PutUnsignedInt(v));
            RegisterPropertyType<long>((v, b) => b.PutLong(v));
            RegisterPropertyType<ulong>((v, b) => b.PutUnsignedLong(v));
            RegisterPropertyType<float>((v, b) => b.PutFloat(v));
            RegisterPropertyType<double>((v, b) => b.PutDouble(v));
            RegisterPropertyType<string>((v, b) => b.PutString(v));
            RegisterPropertyType<Chat>((v, b) => b.PutChat(v));
            RegisterPropertyType<VarInt>((v, b) => b.PutVarInt(v));
            RegisterPropertyType<VarLong>((v, b) => b.PutVarLong(v));
            RegisterPropertyType<Position>((v, b) => b.PutPosition(v));
            RegisterPropertyType<Angle>((v, b) => b.PutAngle(v));
            RegisterPropertyType<Guid>((v, b) => b.PutGuid(v));
            RegisterPropertyType<ByteArray>((v, b) => b.PutByteArray(v));
        }

        private void RegisterPropertyType<T>(Action<T, IPacketBuilder> func)
        {
            _propertyTypes[typeof(T)] = (v, r) => func((T)v, r);
        }

        private void SerializeEnum(object value, IPacketBuilder builder)
        {
            builder.PutVarInt(new VarInt((int)value));
        }
    }
}
