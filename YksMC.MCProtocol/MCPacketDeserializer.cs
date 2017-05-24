using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using YksMC.MCProtocol.Models.Packets;
using YksMC.MCProtocol.Models.Types;

namespace YksMC.MCProtocol
{
    public class MCPacketDeserializer : IMCPacketDeserializer
    {
        private readonly Dictionary<int, Type> _packetTypes;
        private readonly Dictionary<Type, Func<IMCPacketReader, object>> _propertyTypes;

        public MCPacketDeserializer()
        {
            _packetTypes = new Dictionary<int, Type>();
            _propertyTypes = new Dictionary<Type, Func<IMCPacketReader, object>>();

            RegisterPropertyTypes();
        }


        public AbstractPacket Deserialize(IMCPacketReader reader)
        {
            VarInt packetId = reader.GetVarInt();
            Type type;
            if (!_packetTypes.TryGetValue(packetId.Value, out type))
                throw new ArgumentException("Unsupported packet id: " + packetId.Value);
            AbstractPacket packet = (AbstractPacket)type.GetTypeInfo().GetConstructor(new Type[0]).Invoke(new object[0]);
            packet.Id = packetId;

            foreach (PropertyInfo property in type.GetRuntimeProperties())
                if (property.Name != nameof(AbstractPacket.Id))
                    property.SetValue(packet, _propertyTypes[property.PropertyType](reader));

            return packet;
        }

        public void RegisterType<T>(int id) where T : AbstractPacket
        {
            Type type = typeof(T);
            foreach (PropertyInfo property in type.GetRuntimeProperties())
            {
                if (!_propertyTypes.ContainsKey(property.PropertyType))
                    throw new ArgumentException($"Unsupported property type: {property.PropertyType}");
            }
            _packetTypes.Add(id, type);
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
