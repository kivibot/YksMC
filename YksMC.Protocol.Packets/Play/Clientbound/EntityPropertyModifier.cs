using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    public class EntityPropertyModifier
    {
        public Guid Uuid { get; set; }
        public double Amount { get; set; }
        public byte Operation { get; set; }
    }
}
