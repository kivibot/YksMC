using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YksMC.Bot.Login
{
    public interface ILoginService
    {
        Task<IPlayerInfo> LoginAsync();
    }
}
