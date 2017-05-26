using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Clients.Models.Dtos
{
    public class PlayersDto
    {
        public int Max { get; set; }
        public int Online { get; set; }
        public List<PlayerDto> Sample { get; set; }
    }
}
