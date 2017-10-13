using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Dimension
{
    public class AgeTime : IAgeTime
    {
        private readonly long _ageTicks;
        private readonly long _timeTicks;
        private readonly long _ticksPerDay;

        public long AgeTicks => _ageTicks;
        public long TimeTicks => _timeTicks;
        public double AgeDays => _ageTicks / (double)_ticksPerDay;
        public double TimeDays => _timeTicks / (double)_ticksPerDay;

        public AgeTime(long ageTicks, long timeTicks, long ticksPerDay)
        {
            _ageTicks = ageTicks;
            _timeTicks = timeTicks % ticksPerDay;
            _ticksPerDay = ticksPerDay;
        }

        public IAgeTime ChangeAgeAndTime(long ageTicks, long timeTicks)
        {
            return new AgeTime(ageTicks, timeTicks, _ticksPerDay);
        }
    }
}
