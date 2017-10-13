using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Player
{
    internal class ExperienceInfo
    {
        private readonly int _level;
        private readonly double _levelProgress;
        private readonly int _totalExperience;

        public int Level => _level;
        public double LevelProgress => _levelProgress;
        public int TotalExperience => _totalExperience;

        public ExperienceInfo(int level, double levelProgress, int totalExperience)
        {
            if (level < 0)
            {
                throw new ArgumentException(nameof(level));
            }
            if (levelProgress < 0)
            {
                throw new ArgumentException(nameof(levelProgress));
            }
            if (totalExperience < 0)
            {
                throw new ArgumentException(nameof(totalExperience));
            }
            _level = level;
            _levelProgress = levelProgress;
            _totalExperience = totalExperience;
        }
    }
}
