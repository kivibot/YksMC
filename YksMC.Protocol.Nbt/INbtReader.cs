using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Nbt.Models;

namespace YksMC.Protocol.Nbt
{
    public interface INbtReader
    {
        T GetTag<T>() where T : BaseTag;
    }
}
