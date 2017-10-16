using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Urge
{
    public class Urge : IUrge
    {
        private readonly string _name;
        private readonly string _taskName;
        private readonly IEnumerable<IUrgeScorer> _scorers;
        private readonly IEnumerable<IUrgeCondition> _conditions;

        public string Name => _name;
        public string TaskName => _taskName;
        public IEnumerable<IUrgeScorer> Scorers => _scorers;
        public IEnumerable<IUrgeCondition> Conditions => _conditions;

        public Urge(string name, string taskName, IEnumerable<IUrgeScorer> scorers, IEnumerable<IUrgeCondition> conditions)
        {
            _name = name;
            _taskName = taskName;
            _scorers = scorers;
            _conditions = conditions;
        }

        public double GetScore(IWorld world)
        {
            return _scorers.Select(scorer => scorer.GetScore(world))
                .Sum();
        }

        public bool IsPossible(IWorld world)
        {
            return !_conditions.Select(condition => condition.IsPossible(world)).Any(isPossible => !isPossible);
        }
    }
}
