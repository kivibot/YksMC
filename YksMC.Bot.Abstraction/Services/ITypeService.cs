using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Models;

namespace YksMC.Abstraction.Services
{
    public interface ITypeService
    {
        IBlockType GetBlockType(int blockId, byte blockMetadata);
    }
}
