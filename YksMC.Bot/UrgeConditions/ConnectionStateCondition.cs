using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Behavior.Urge;
using YksMC.Client;
using YksMC.MinecraftModel.World;
using YksMC.Protocol.Models.Constants;

namespace YksMC.Bot.UrgeConditions
{
    public class ConnectionStateCondition : IUrgeCondition
    {
        private readonly IMinecraftClient _minecraftClient;
        private readonly ConnectionState _targetState;

        public string Name => $"Connection state: {_targetState}";

        public ConnectionStateCondition(IMinecraftClient minecraftClient, ConnectionState targetState)
        {
            _minecraftClient = minecraftClient;
            _targetState = targetState;
        }

        public bool IsPossible(IWorld world)
        {
            return _minecraftClient.State == _targetState;
        }
    }
}
