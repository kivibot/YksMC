﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Client.Models.Status
{
    public class ServerStatus
    {
        public DescriptionDto Description { get; set; }
        public StatusPlayers Players { get; set; }
        public StatusVersion Version { get; set; }
        public string Favicon { get; set; }

        public TimeSpan Ping { get; set; }
    }
}