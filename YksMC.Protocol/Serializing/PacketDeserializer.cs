using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using YksMC.Protocol.Models;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Serializing
{
    public class PacketDeserializer : IPacketDeserializer
    {
        private readonly Dictionary<Type, Func<IPacketReader, object>> _propertyTypes;

        public PacketDeserializer()
        {
            _propertyTypes = new Dictionary<Type, Func<IPacketReader, object>>();

            RegisterPropertyTypes();
        }

        public T Deserialize<T>(IPacketReader reader)
        {
            Type type = typeof(T);
            return (T)Deserialize(reader, type);
        }

        public object Deserialize(IPacketReader reader, Type type)
        {
            Func<IPacketReader, object> deserializeProperty;

            if (!_propertyTypes.TryGetValue(type, out deserializeProperty))
            {
                TypeInfo typeInfo = type.GetTypeInfo();
                if (typeInfo.IsEnum)
                    deserializeProperty = (r) => DeserializeEnum(r, type);
                else if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(VarArray<>))
                    deserializeProperty = (r) => DeserializeVarArray(r, typeInfo);
                else if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Optional<>))
                    deserializeProperty = (r) => DeserializeOptional(r, typeInfo);
                else if (typeInfo.IsClass)
                    deserializeProperty = (r) => DeserializeObject(r, type);
                else
                    throw new ArgumentException($"Unsupported property type: {type}");
            }

            return deserializeProperty(reader);
        }

        private object DeserializeObject(IPacketReader reader, Type type)
        {
            object value = type.GetTypeInfo().GetConstructor(new Type[0]).Invoke(new object[0]);

            foreach (PropertyInfo property in type.GetRuntimeProperties())
            {
                if (!CheckConditional(value, property))
                    continue;

                property.SetValue(value, Deserialize(reader, property.PropertyType));
            }

            return value;
        }

        private bool CheckConditional(object obj, PropertyInfo property)
        {
            ConditionalAttribute conditional = property.GetCustomAttribute<ConditionalAttribute>();
            if (conditional == null)
                return true;

            object referencedValue = obj.GetType().GetTypeInfo().GetProperty(conditional.Field).GetValue(obj);

            if (conditional.Condition == Condition.Is)
                return conditional.Values.Contains(referencedValue);
            return !conditional.Values.Contains(referencedValue);                            
        }

        private void RegisterPropertyTypes()
        {
            RegisterPropertyType<bool>((r) => r.GetBool());
            RegisterPropertyType<sbyte>((r) => r.GetSignedByte());
            RegisterPropertyType<byte>((r) => r.GetByte());
            RegisterPropertyType<short>((r) => r.GetShort());
            RegisterPropertyType<ushort>((r) => r.GetUnsignedShort());
            RegisterPropertyType<int>((r) => r.GetInt());
            RegisterPropertyType<uint>((r) => r.GetUnsignedInt());
            RegisterPropertyType<long>((r) => r.GetLong());
            RegisterPropertyType<ulong>((r) => r.GetUnsignedLong());
            RegisterPropertyType<float>((r) => r.GetFloat());
            RegisterPropertyType<double>((r) => r.GetDouble());
            RegisterPropertyType<string>((r) => r.GetString());
            RegisterPropertyType<Chat>((r) => r.GetChat());
            RegisterPropertyType<VarInt>((r) => r.GetVarInt());
            RegisterPropertyType<VarLong>((r) => r.GetVarLong());
            RegisterPropertyType<Position>((r) => r.GetPosition());
            RegisterPropertyType<Angle>((r) => r.GetAngle());
            RegisterPropertyType<Guid>((r) => r.GetGuid());
        }

        private void RegisterPropertyType<T>(Func<IPacketReader, T> func)
        {
            _propertyTypes[typeof(T)] = (r) => (T)func(r);
        }

        private object DeserializeEnum(IPacketReader reader, Type type)
        {
            int value = reader.GetVarInt().Value;
            if (!Enum.IsDefined(type, value))
                throw new ArgumentException($"Unknown enum value! type: {type.Name}, value: {value}");
            return value;
        }

        private object DeserializeVarArray(IPacketReader reader, TypeInfo typeInfo)
        {
            VarInt length = Deserialize<VarInt>(reader);
            Type valueType = typeInfo.GetGenericArguments()[0];
            Array values = DeserializeArray(reader, length.Value, valueType);

            TypeInfo genericType = typeof(VarArray<>).MakeGenericType(valueType).GetTypeInfo();
            object varArray = genericType.GetConstructor(new Type[0]).Invoke(new object[0]);
            genericType.GetProperty(nameof(VarArray<byte>.Count)).SetValue(varArray, length);
            genericType.GetProperty(nameof(VarArray<byte>.Values)).SetValue(varArray, values);

            return varArray;
        }

        private Array DeserializeArray(IPacketReader reader, int length, Type valueType)
        {
            if (valueType == typeof(byte))
            {
                return reader.GetBytes(length);
            }

            Array values = Array.CreateInstance(valueType, length);
            for (int i = 0; i < length; i++)
                values.SetValue(Deserialize(reader, valueType), i);

            return values;
        }

        private VarArray<T> CreateVarArray<T>(VarInt count, object[] values, T dummyObject)
        {
            return new VarArray<T>()
            {
                Count = count,
                Values = values.Cast<T>().ToArray()
            };
        }
        private object DeserializeOptional(IPacketReader reader, TypeInfo typeInfo)
        {
            Type valueType = typeInfo.GetGenericArguments()[0];
            bool hasValue = reader.GetBool();

            TypeInfo genericType = typeof(Optional<>).MakeGenericType(valueType).GetTypeInfo();
            object varArray = genericType.GetConstructor(new Type[0]).Invoke(new object[0]);
            genericType.GetProperty(nameof(Optional<byte>.HasValue)).SetValue(varArray, hasValue);

            if (hasValue)
            {
                object value = Deserialize(reader, valueType);
                genericType.GetProperty(nameof(Optional<byte>.Value)).SetValue(varArray, value);
            }

            return varArray;
        }
    }
}
