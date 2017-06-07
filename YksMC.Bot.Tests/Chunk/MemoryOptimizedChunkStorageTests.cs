using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Models;
using YksMC.Bot.Chunk.MemoryOptimized;

namespace YksMC.Bot.Tests.Chunk
{
    [TestFixture]
    public class MemoryOptimizedChunkStorageTests
    {
        public void SetBlockLight_ChunkNotLoaded_DoesNothing()
        {
            MemoryOptimizedChunkStorage storage = new MemoryOptimizedChunkStorage();
            storage.SetBlockLight(new BlockLocation(Dimension.Overworld, 0, 0, 0), 15);
        }
        public void SetBlockLight_ChunkLoaded_SavesTheValue()
        {
            MemoryOptimizedChunkStorage storage = new MemoryOptimizedChunkStorage();

            storage.SetBlockLight(new BlockLocation(Dimension.Overworld, 0, 0, 0), 15);
            IBlock block = storage.GetBlock();

            Assert.AreEqual(15, block.BlockLight);
        }

    }
}
