﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Chunk;

namespace YksMC.Behavior.Tasks
{
    public class BreakBlockCommand 
    {
        public IBlockLocation Location { get; private set; }

        public BreakBlockCommand(IBlockLocation location)
        {
            Location = location;
        }
    }
}
