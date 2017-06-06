﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Models;

namespace YksMC.Abstraction.Services
{
    public interface IChunkService
    {
        IChunk GetChunk(Dimension dimension, int x, int z);
        IChunk CreateChunk(Dimension dimension, int x, int z);
        IBlock GetBlock(BlockLocation location);

        void SetBlockType(BlockLocation location, IBlockType type);
        void SetBlockLight(BlockLocation location, byte level);
        void SetSkyLight(BlockLocation location, byte level);
    }
}
