using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Urge
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
            return _urges.Where(urge => urge.IsPossible(world))
                .OrderByDescending(urge => urge.GetScore(world))
                .FirstOrDefault();
        }
    }
}
