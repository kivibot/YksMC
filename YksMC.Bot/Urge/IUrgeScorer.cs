using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.Urge
{
    public interface IUrgeScorer
    {
        string Name { get; }

        double GetScore(IWorld world);
    }
}
