using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Data.Json.EntityType
{
    public class JsonEntityType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public JsonEntityTypeType Type { get; set; }
    }
}
