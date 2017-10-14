using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Bot.Login
{
    public interface IPlayerInfo
    {
        string Id { get; }
        string Username { get; }
    }
}
