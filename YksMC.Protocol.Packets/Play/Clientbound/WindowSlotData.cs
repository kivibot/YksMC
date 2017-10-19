using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;
using YksMC.Protocol.Nbt.Models;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    public class WindowSlotData
    {
        public short BlockId { get; set; }

        [Conditional(nameof(BlockId), Condition.IsNot, (short)-1)]
        public byte ItemCount { get; set; }
        [Conditional(nameof(BlockId), Condition.IsNot, (short)-1)]
        public short ItemDamage { get; set; }
        [Conditional(nameof(BlockId), Condition.IsNot, (short)-1)]
        public BaseTag Nbt { get; set; }
    }
}
