using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound.PlayerListItem
{
    public class PlayerListItemProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Optional<string> Signature { get; set; }
    }
}
