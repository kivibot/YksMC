﻿using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Login;
using YksMC.Bot.BehaviorTask;
using YksMC.Client;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;
using YksMC.Bot.WorldEvent;
using YksMC.Bot.Core;

namespace YksMC.Behavior.Tasks
{
    [Obsolete("uses task.wait!!!")]
    public class LoginTask : BehaviorTask<LoginCommand>
    {
        private readonly IMinecraftClient _minecraftClient;
        private readonly ILoginService _loginService;

        public override string Name => "Login";

        public LoginTask(LoginCommand command, ILoginService loginService, IMinecraftClient minecraftClient)
            : base(command)
        {
            _loginService = loginService;
            _minecraftClient = minecraftClient;
        }

        public override IWorldEventResult OnStart(IWorld world)
        {
            _minecraftClient.ConnectAsync("localhost", 25565).Wait();

            IPlayerInfo playerInfo = _loginService.LoginAsync().Result;

            Complete();
            //TODO: should this be removed from here?
            return Result(world.ReplaceLocalPlayer(new Player(Guid.Parse(playerInfo.Id), playerInfo.Username)));
        }

        public override void OnTick(IWorld world, IGameTick tick)
        {
            return;
        }
    }
}
