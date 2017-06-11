using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Bot;
using YksMC.Abstraction.Misc;

namespace YksMC.Bot.Bot
{
    public class MinecraftBot : IMinecraftBot
    {
        public Guid UserId => _userId;
        public string UserName => _username;
        public IPlayer Player => _player;

        private Guid _userId;
        private string _username;
        private IPlayer _player;

        public void SetPlayer(IPlayer player)
        {
            _player = player;
        }

        public void SetUserInfo(Guid userId, string username)
        {
            _userId = userId;
            _username = username;
        }
    }
}
