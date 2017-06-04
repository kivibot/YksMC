using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Bot.Models
{
    public class Block
    {
        public BlockType Type { get; set; }
        public byte BlockLight { get; set; }
        public byte SkyLight { get; set; }

        public Block(BlockType type, byte blockLight, byte skyLight)
        {
            Type = type;
            BlockLight = blockLight;
            SkyLight = skyLight;
        }
    }
}
