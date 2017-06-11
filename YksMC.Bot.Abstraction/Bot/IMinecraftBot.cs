using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Abstraction.Misc;

namespace YksMC.Abstraction.Bot
{
    public interface IMinecraftBot
    {
        Guid UserId { get; }
        string UserName { get; }
        IPlayer Player { get; }

        void SetUserInfo(Guid userId, string username);
        void SetPlayer(IPlayer player);
    }
}
