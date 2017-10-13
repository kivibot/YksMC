using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Dimension
{
    public interface IAgeTime
    {
        ulong AgeTicks { get; }
        ulong TimeTicks { get; }

        double AgeDays { get; }
        double TimeDays { get; }

        IAgeTime ChangeAgeAndTime(ulong ageTicks, ulong timeTicks);
    }
}
