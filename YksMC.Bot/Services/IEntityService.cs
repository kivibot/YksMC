using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Protocol.Models.Types;

namespace YksMC.Bot.Services
{
    public interface IEntityService
    {
        void CreatePlayer();
        void Remove(IEnumerable<VarInt> ids);
    }
}
