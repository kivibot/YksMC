using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Data.Json.BlockType
{
    public class JsonBlockType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSolid { get; set; }
        public bool IsDiggable { get; set; }
        public double Hardness { get; set; }
        public int Tier { get; set; }
        public string Material { get; set; }
        public bool IsDangerous { get; set; }
    }
}
