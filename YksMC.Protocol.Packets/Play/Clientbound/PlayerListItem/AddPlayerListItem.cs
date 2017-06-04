using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;

namespace YksMC.Protocol.Packets.Play.Clientbound.PlayerListItem
{
    public class AddPlayerListItem
    {
        public Guid UniqueId { get; set; }
        public string Name { get; set; }
        public VarArray<VarInt, PlayerListItemProperty> Properties { get; set; }
        public VarInt Gamemode { get; set; }
        public VarInt Ping { get; set; }
        public Optional<Chat> DisplayName { get; set; }
    }
}
