using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Bot.GameObjectRegistry
{
    public class GameObjectRegistry<TBase> : IGameObjectRegistry<TBase> where TBase : class
    {
        private readonly IDictionary<int, TBase> _objectsById;
        private readonly IDictionary<string, TBase> _objectsByName;

        public GameObjectRegistry()
        {
            _objectsById = new Dictionary<int, TBase>();
            _objectsByName = new Dictionary<string, TBase>();
        }

        public T Get<T>(int id) where T : class, TBase
        {
            return _objectsById[id] as T;
        }

        public T Get<T>(string name) where T : class, TBase
        {
            return _objectsByName[name] as T;
        }

        public void Register(TBase gameObject, int id)
        {
            if (_objectsById.ContainsKey(id))
            {
                throw new ArgumentException($"Id collision: {id}");
            }
            _objectsById[id] = gameObject;
        }

        public void Register(TBase gameObject, string name)
        {
            if (_objectsByName.ContainsKey(name))
            {
                throw new ArgumentException($"Name collision: {name}");
            }
            _objectsByName[name] = gameObject;
        }

        public void Register(TBase gameObject, int id, string name)
        {
            Register(gameObject, id);
            Register(gameObject, name);
        }
    }
}
