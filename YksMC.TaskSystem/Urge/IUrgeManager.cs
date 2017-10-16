using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Behavior.Urge
{
    public interface IUrgeManager
    {
        void AddUrge(IUrge urge);

        IUrge GetLargestUrge(IWorld world);
    }
}
