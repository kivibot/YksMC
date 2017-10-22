using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Login;
using YksMC.Bot.BehaviorTask;
using YksMC.Client;
using YksMC.MinecraftModel.Player;
using YksMC.MinecraftModel.World;
using YksMC.Bot.WorldEvent;
using YksMC.Bot.Core;
using YksMC.Protocol.Models.Constants;
using System.Threading.Tasks;

namespace YksMC.Behavior.Tasks
{
    [Obsolete("uses task.wait!!!")]
    public class LoginTask : BehaviorTask<LoginCommand>
    {
        private readonly ILoginService _loginService;

        public override string Name => "Login";

        private IPlayerInfo _playerInfo;

        public LoginTask(LoginCommand command, IMinecraftClient minecraftClient, IBehaviorTaskScheduler taskScheduler, ILoginService loginService) 
            : base(command, minecraftClient, taskScheduler)
        {
            _loginService = loginService;
        }

        public override bool IsPossible(IWorld world)
        {
            return _minecraftClient.State == ConnectionState.None;
        }

        public override IBehaviorTaskEventResult OnStart(IWorld world)
        {
            LoginAsync().GetAwaiter().GetResult();
            world = world.ReplaceLocalPlayer(new Player(Guid.Parse(_playerInfo.Id), _playerInfo.Username));
            return Success(world);
        }

        private async Task LoginAsync()
        {
            await _minecraftClient.ConnectAsync("localhost", 25565);
            _playerInfo = await _loginService.LoginAsync();
        }

        public override IBehaviorTaskEventResult OnTick(IWorld world, IGameTick tick)
        {
            return Result(world);
        }
    }
}
