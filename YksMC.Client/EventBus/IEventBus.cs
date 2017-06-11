﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Protocol.Models;

namespace YksMC.Client.EventBus
{
    public interface IEventBus
    {
        void DispatchEvent(object message);
    }
}