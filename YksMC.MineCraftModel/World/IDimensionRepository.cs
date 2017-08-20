using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.World
{
    public interface IDimensionRepository
    {
        IDimension GetDimension(int id);
    }
}
