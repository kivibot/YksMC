using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Packets;
using YksMC.Protocol.Models.Types;
using System.Reflection;

namespace YksMC.Protocol.Serializing
{
    public class MCPacketSerializer : IMCPacketSerializer
    {
        private readonly Dictionary<Type, Action<IMCPacketWriter, object>> _propertyTypes;

        public MCPacketSerializer()
        {
            _propertyTypes = new Dictionary<Type, Action<IMCPacketWriter, object>>();

            RegisterPropertyTypes();
        }

        public void Serialize(object packet, IMCPacketWriter writer)
        {
            if (packet == null)
                throw new ArgumentNullException(nameof(packet));

            Type type = packet.GetType();
            foreach (PropertyInfo property in type.GetRuntimeProperties())
            {
                Action<IMCPacketWriter, object> serializeProperty;
                if (property.PropertyType.GetTypeInfo().IsEnum)
                    serializeProperty = SerializeEnum;
                else if (!_propertyTypes.TryGetValue(property.PropertyType, out serializeProperty))
                    throw new ArgumentException($"Unsupported property type: {property.PropertyType}");

                object value = property.GetValue(packet);
                if (value == null)
                    throw new ArgumentException($"Null property: {property.Name}");

                serializeProperty(writer, value);
            }
        }

        private void RegisterPropertyTypes()
        {
            RegisterPropertyType<bool>((r, v) => r.PutBool(v));
            RegisterPropertyType<sbyte>((r, v) => r.PutSignedByte(v));
            RegisterPropertyType<byte>((r, v) => r.PutByte(v));
            RegisterPropertyType<short>((r, v) => r.PutShort(v));
            RegisterPropertyType<ushort>((r, v) => r.PutUnsignedShort(v));
            RegisterPropertyType<int>((r, v) => r.PutInt(v));
            RegisterPropertyType<uint>((r, v) => r.PutUnsignedInt(v));
            RegisterPropertyType<long>((r, v) => r.PutLong(v));
            RegisterPropertyType<ulong>((r, v) => r.PutUnsignedLong(v));
            RegisterPropertyType<float>((r, v) => r.PutFloat(v));
            RegisterPropertyType<double>((r, v) => r.PutDouble(v));
            RegisterPropertyType<string>((r, v) => r.PutString(v));
            RegisterPropertyType<Chat>((r, v) => r.PutChat(v));
            RegisterPropertyType<VarInt>((r, v) => r.PutVarInt(v));
            RegisterPropertyType<VarLong>((r, v) => r.PutVarLong(v));
            RegisterPropertyType<Position>((r, v) => r.PutPosition(v));
            RegisterPropertyType<Angle>((r, v) => r.PutAngle(v));
            RegisterPropertyType<Guid>((r, v) => r.PutGuid(v));
        }

        private void RegisterPropertyType<T>(Action<IMCPacketWriter, T> func)
        {
            _propertyTypes[typeof(T)] = (r, v) => func(r, (T)v);
        }

        private void SerializeEnum(IMCPacketWriter writer, object value)
        {
            writer.PutVarInt(new VarInt((int)value));
        }
    }
}
