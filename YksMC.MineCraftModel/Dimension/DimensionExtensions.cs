using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Dimension
{
    public static class DimensionExtensions
    {
        public static IDimension ChangeAgeAndTime(this IDimension dimension, Func<IAgeTime, IAgeTime> getAgeAndTime)
        {
            return dimension.ChangeAgeAndTime(getAgeAndTime(dimension.AgeAndTime));
        }
    }
}
