﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;
using System.Reflection;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using System.Linq;

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

        public void Serialize(object value, IPacketBuilder builder)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Action<object, IPacketBuilder> serializeProperty;
            if (!_propertyTypes.TryGetValue(value.GetType(), out serializeProperty))
            {
                TypeInfo valueTypeInfo = value.GetType().GetTypeInfo();
                if (valueTypeInfo.IsEnum)
                    serializeProperty = SerializeEnum;
                else if (valueTypeInfo.IsGenericType && valueTypeInfo.GetGenericTypeDefinition() == typeof(VarArray<,>))
                    serializeProperty = SerializeVarArray;
                else if (valueTypeInfo.IsClass)
                    serializeProperty = SerializeObject;
                else
                    throw new ArgumentException($"Unsupported property type: {valueTypeInfo.Name}");
            }

            serializeProperty(value, builder);
        }

        private void SerializeObject(object value, IPacketBuilder builder)
        {
            Type type = value.GetType();
            foreach (PropertyInfo property in type.GetRuntimeProperties())
            {
                if (!CheckConditional(value, property))
                    continue;

                object propertyValue = property.GetValue(value);
                Serialize(propertyValue, builder);
            }
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
        }

        private void RegisterPropertyType<T>(Action<T, IPacketBuilder> func)
        {
            _propertyTypes[typeof(T)] = (v, r) => func((T)v, r);
        }

        private void SerializeEnum(object value, IPacketBuilder builder)
        {
            builder.PutVarInt(new VarInt((int)value));
        }

        private void SerializeVarArray(object value, IPacketBuilder builder)
        {
            TypeInfo typeInfo = value.GetType().GetTypeInfo();
            Type lengthType = typeInfo.GetGenericArguments()[0];
            Type valueType = typeInfo.GetGenericArguments()[1];

            object count = typeInfo.GetProperty(nameof(VarArray<byte, byte>.Count)).GetValue(value);
            Array values = (Array)typeInfo.GetProperty(nameof(VarArray<byte, byte>.Values)).GetValue(value);

            Serialize(count, builder);
            if (valueType == typeof(byte))
            {
                byte[] data = new byte[values.Length];
                values.CopyTo(data, 0);
                builder.PutBytes(data);
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                    Serialize(values.GetValue(i), builder);
            }
        }
    }
}
