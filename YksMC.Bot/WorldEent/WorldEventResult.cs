using System;
using System.Collections.Generic;
using System.Text;
using YksMC.MinecraftModel.Dimension;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.WorldEvent
{
    public class WorldEventResult : IWorldEventResult
    {
        private readonly IWorld _world;
        private readonly IReadOnlyList<object> _replyPackets;

        public IWorld World => _world;
        public IReadOnlyList<object> ReplyPackets => _replyPackets;

        public WorldEventResult(IWorld world, IReadOnlyList<object> replyPackets)
        {
            _world = world;
            _replyPackets = replyPackets;
        }
    }
}
