using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Bot.Login
{
    internal class PlayerInfo : IPlayerInfo
    {
        private readonly string _id;
        private readonly string _username;

        public string Id => _id;
        public string Username => _username;

        public PlayerInfo(string id, string username)
        {
            _id = id;
            _username = username;
        }
    }
}
