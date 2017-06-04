using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    public class EntityProperty
    {
        public string Key { get; set; }
        public double Value { get; set; }
        public VarArray<VarInt, EntityPropertyModifier> Modifiers { get; set; }

    }
}