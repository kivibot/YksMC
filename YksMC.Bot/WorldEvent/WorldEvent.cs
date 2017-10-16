using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.WorldEvent
{
    public class WorldEvent<T> : IWorldEvent<T>
    {
        private readonly IWorld _world;
        private readonly T _event;

        public IWorld World => _world;
        public T Event => _event;

        public WorldEvent(IWorld world, T eventArgs)
        {
            _world = world;
            _event = eventArgs;
        }
    }
}
