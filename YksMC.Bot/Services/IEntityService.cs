﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Models;
using YksMC.Protocol.Models.Types;

namespace YksMC.Bot.Services
{
    public interface IEntityService
    {
        PlayerEntity GetLocalPlayer();
    }
}
