using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.Urge
{
    public class UrgeManager : IUrgeManager
    {
        private readonly IList<IUrge> _urges;

        public UrgeManager()
        {
            _urges = new List<IUrge>();
        }

        public void AddUrge(IUrge urge)
        {
            _urges.Add(urge);
        }

        public IUrge GetLargestUrge(IWorld world)
        {
            double largestScore = double.MinValue;
            IUrge largestUrge = null;
            foreach(IUrge urge in _urges)
            {
                if (!urge.IsPossible(world))
                {
                    continue;
                }
                double score = urge.GetScore(world);
                if(score < largestScore)
                {
                    continue;
                }
                largestUrge = urge;
                largestScore = score;
            }
            return largestUrge;
        }
    }
}
