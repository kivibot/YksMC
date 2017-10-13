using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Dimension
{
    public interface IAgeTime
    {
        long AgeTicks { get; }
        long TimeTicks { get; }

        double AgeDays { get; }
        double TimeDays { get; }

        IAgeTime ChangeAgeAndTime(long ageTicks, long timeTicks);
    }
}
