using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Core;
using YksMC.Bot.WorldEvent;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.TickHandlers
{
    public class PlayerMovementHandler : WorldEventHandler, IWorldEventHandler<IGameTick>
    {
        private const double _acceleration = 0.08;
        private const double _drag = 0.02;
        private const double _maxVelocity = 3.92;

        public IWorldEventResult ApplyEvent(IGameTick tick, IWorld world)
        {

        }
    }
}
