﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Window
{
    public interface IWindowTypeRepository
    {
        IWindowType Get(string id);
    }
}