using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Bot.Models
{
    public class ChunkSection
    {
        public const int Width = 16;
        public const int Height = 16;

        public Block[,,] Blocks { get; set; }
    }
}
