using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Dimension
{
    public interface IDimensionTypeRepository
    {
        IDimensionType GetDimensionType(int id);
    }
}
