using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Urge
{
    public interface IUrge
    {
        string Name { get; }
        string TaskName { get; }
        IEnumerable<IUrgeScorer> Scorers { get; }
        IEnumerable<IUrgeCondition> Conditions { get; }
        
        double GetScore(IWorld world);
        bool IsPossible(IWorld world);
    }
}
