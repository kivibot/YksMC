using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;

namespace YksMC.Bot.Models
{
    public class Entity
    {
        public Guid UniqueId { get; set; }
        public VarInt Id { get; set; }
    }
}
