using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    public class EntityMetadata
    {
        public byte Index { get; set; }
        public byte Type { get; set; }

        [Conditional(nameof(Type), Condition.Is, (byte)0)]
        public byte ByteValue { get; set; }
        [Conditional(nameof(Type), Condition.Is, (byte)1)]
        public VarInt VarIntValue { get; set; }
        [Conditional(nameof(Type), Condition.Is, (byte)2)]
        public float FloatValue { get; set; }
        [Conditional(nameof(Type), Condition.Is, (byte)3)]
        public string StringValue { get; set; }
        [Conditional(nameof(Type), Condition.Is, (byte)4)]
        public Chat ChatValue { get; set; }
        [Conditional(nameof(Type), Condition.Is, (byte)5)]
        public WindowSlot SlotValue { get; set; }
        [Conditional(nameof(Type), Condition.Is, (byte)6)]
        public bool BooleanValue { get; set; }
        [Conditional(nameof(Type), Condition.Is, (byte)7)]
        public Vector<float> RotationValue { get; set; }
        [Conditional(nameof(Type), Condition.Is, (byte)8)]
        public Position PositionValue { get; set; }
        [Conditional(nameof(Type), Condition.Is, (byte)9)]
        public Optional<Position> OptPositionValue { get; set; }
        [Conditional(nameof(Type), Condition.Is, (byte)10)]
        public VarInt DirectionValue { get; set; }
        [Conditional(nameof(Type), Condition.Is, (byte)11)]
        public Optional<Guid> OptGuidValue { get; set; }
        [Conditional(nameof(Type), Condition.Is, (byte)12)]
        public Optional<VarInt> OptBlockIdValue { get; set; }
    }
}
