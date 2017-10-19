using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Bot.GameObjectRegistry
{
    public interface IGameObjectRegistry<T>
    {
        void Register(T gameObject, int id);
        void Register(T gameObject, string name);
        void Register(T gameObject, int id, string name);

        T Get(int id);
        T Get(string name);
    }
}
