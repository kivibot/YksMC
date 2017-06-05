using System;
using System.Collections.Generic;
using System.Text;
using YksMC.Bot.Models;

namespace YksMC.Bot.Services
{
    public class EntityService : IEntityService
    {
        public PlayerEntity GetLocalPlayer()
        {
            return new PlayerEntity()
            {
                Dimension = Dimension.Overworld
            };
        }
    }
}
