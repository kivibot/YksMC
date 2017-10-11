using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace YksMC.MinecraftModel.EntityType
{
    [DebuggerDisplay("{Name}")]
    public class EntityType : IEntityType
    {
        private readonly string _name;

        public string Name => _name;

        public EntityType(string name)
        {
            _name = name;
        }
    }
}
