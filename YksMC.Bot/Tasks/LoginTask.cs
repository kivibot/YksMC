using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Behavior.Task;
using YksMC.Bot.Login;
using YksMC.Client;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;

namespace YksMC.Bot.Tasks
{
    public class LoginTask : IBehaviorTask
    {
        private readonly IMinecraftClient _minecraftClient;
        private readonly ILoginService _loginService;

        public string Name => "Login";

        public bool IsCompleted { get; set; }
        public bool IsFailed { get; set; }

        public LoginTask(ILoginService loginService, IMinecraftClient minecraftClient)
        {
            _loginService = loginService;
            _minecraftClient = minecraftClient;
        }

        public IWorld OnStart(IWorld world)
        {
            _minecraftClient.ConnectAsync("localhost", 25565).Wait();

            IPlayerInfo playerInfo = _loginService.LoginAsync().Result;

            IsCompleted = true;

            //TODO: should this be removed from here?
            return world.ReplaceLocalPlayer(new Player(Guid.Parse(playerInfo.Id), playerInfo.Username));
        }

        public IWorld OnTick(IWorld world)
        {
            return world;
        }
    }
}
