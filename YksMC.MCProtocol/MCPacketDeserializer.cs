using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using YksMC.Protocol.Models.Packets;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol
{
    public class MCPacketDeserializer : IMCPacketDeserializer
    {
        private readonly Dictionary<Type, Func<IMCPacketReader, object>> _propertyTypes;

        public MCPacketDeserializer()
        {
            _propertyTypes = new Dictionary<Type, Func<IMCPacketReader, object>>();

            RegisterPropertyTypes();
        }

        public T Deserialize<T>(IMCPacketReader reader) where T : AbstractPacket
        {
            Type type = typeof(T);
            T packet = (T)type.GetTypeInfo().GetConstructor(new Type[0]).Invoke(new object[0]);

            foreach (PropertyInfo property in type.GetRuntimeProperties())
            {
                Func<IMCPacketReader, object> deserializeProperty;
                if (!_propertyTypes.TryGetValue(property.PropertyType, out deserializeProperty))
                    throw new ArgumentException($"Unsupported property type: {property.PropertyType}");

                property.SetValue(packet, deserializeProperty(reader));
            }

            return packet;
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

        private void RegisterPropertyType<T>(Func<IMCPacketReader, T> func)
        {
            _propertyTypes[typeof(T)] = (r) => (T)func(r);
        }
    }
}
