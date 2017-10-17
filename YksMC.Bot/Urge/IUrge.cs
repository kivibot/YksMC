using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.Urge
{
    public interface IUrge
    {
        string Name { get; }
        object Command { get; }
        IEnumerable<IUrgeScorer> Scorers { get; }
        IEnumerable<IUrgeCondition> Conditions { get; }
        
        double GetScore(IWorld world);
        bool IsPossible(IWorld world);
    }
}
