using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Dimension
{
    public class AgeTime : IAgeTime
    {
        private readonly ulong _ageTicks;
        private readonly ulong _timeTicks;
        private readonly ulong _ticksPerDay;

        public ulong AgeTicks => _ageTicks;
        public ulong TimeTicks => _timeTicks;
        public double AgeDays => _ageTicks / (double)_ticksPerDay;
        public double TimeDays => _timeTicks / (double)_ticksPerDay;

        public AgeTime(ulong ageTicks, ulong timeTicks, ulong ticksPerDay)
        {
            _ageTicks = ageTicks;
            _timeTicks = timeTicks % ticksPerDay;
            _ticksPerDay = ticksPerDay;
        }

        public IAgeTime ChangeAgeAndTime(ulong ageTicks, ulong timeTicks)
        {
            return new AgeTime(ageTicks, timeTicks, _ticksPerDay);
        }
    }
}
