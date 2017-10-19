using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Window;

namespace YksMC.Data.Json.Window
{
    public class JsonWindowTypeRepository : IWindowTypeRepository
    {
        public IWindowType Get(string id)
        {
            return new WindowType(true, 10, 36);
        }
    }
}
