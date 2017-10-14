using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YksMC.Bot.Handlers;
using YksMC.Bot.Login;
using YksMC.Client;

namespace YksMC.Bot.Core
{
    public class YksClient
    {
        private readonly IMinecraftClient _minecraftClient;
        private readonly ILoginService _loginService;

        public YksClient(IMinecraftClient minecraftClient, ILoginService loginService)
        {
            _minecraftClient = minecraftClient;
            _loginService = loginService;
        }

        public async Task RunAsync()
        {
            await _minecraftClient.ConnectAsync("localhost", 25565);
            IPlayerInfo playerInfo = await _loginService.LoginAsync();
        }
    }
}
