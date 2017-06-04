using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Attributes;
using YksMC.Protocol.Models.Constants;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    public class WindowSlot
    {
        public short BlockId { get; set; }
        public byte ItemCount { get; set; }
        public byte ItemDamage { get; set; }
        //TODO: optional values and nbt tags
    }
}
