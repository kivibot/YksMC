using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MCProtocol.Models
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        
        public Position(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
