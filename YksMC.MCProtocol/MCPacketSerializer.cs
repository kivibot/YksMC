using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MCProtocol.Models.Packets;
using YksMC.MCProtocol.Models.Types;
using System.Reflection;

namespace YksMC.MCProtocol
{
    public class MCPacketSerializer : IMCPacketSerializer
    {
        private readonly Dictionary<Type, Action<IMCPacketWriter, object>> _propertyTypes;

        public MCPacketSerializer()
        {
            _propertyTypes = new Dictionary<Type, Action<IMCPacketWriter, object>>();

            RegisterPropertyTypes();
        }

        public void Serialize<T>(T packet, IMCPacketWriter writer) where T : AbstractPacket
        {
            Type type = packet.GetType();
            foreach (PropertyInfo property in type.GetRuntimeProperties())
            {
                Action<IMCPacketWriter, object> serializeProperty;
                if (!_propertyTypes.TryGetValue(property.PropertyType, out serializeProperty))
                    throw new ArgumentException($"Unsupported property type: {property.PropertyType}");

                serializeProperty(writer, property.GetValue(packet));
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
    }
}
