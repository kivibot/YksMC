using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound
{
    public class Statistic
    {
        public string Name { get; set; }
        public VarInt Value { get; set; }
    }
}
