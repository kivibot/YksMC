using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Clients.Models.Dtos
{
    public class StatusDto
    {
        public DescriptionDto Description { get; set; }
        public PlayersDto Players { get; set; }
        public VersionDto Version { get; set; }
        public string Favicon { get; set; }
    }
}
