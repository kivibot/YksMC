using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Bot.GameObjectRegistry
{
    public interface IGameObjectRegistry<TBase> where TBase : class
    {
        void Register(TBase gameObject, int id);
        void Register(TBase gameObject, string name);
        void Register(TBase gameObject, int id, string name);

        T Get<T>(int id) where T : class, TBase;
        T Get<T>(string name) where T : class, TBase;
    }
}
