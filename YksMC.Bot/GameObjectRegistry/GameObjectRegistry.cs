using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.Bot.GameObjectRegistry
{
    public class GameObjectRegistry<T> : IGameObjectRegistry<T>
    {
        private readonly IDictionary<int, T> _objectsById;
        private readonly IDictionary<string, T> _objectsByName;

        public GameObjectRegistry()
        {
            _objectsById = new Dictionary<int, T>();
            _objectsByName = new Dictionary<string, T>();
        }

        public T Get(int id)
        {
            return _objectsById[id];
        }

        public T Get(string name)
        {
            return _objectsByName[name];
        }

        public void Register(T gameObject, int id)
        {
            if (_objectsById.ContainsKey(id))
            {
                throw new ArgumentException($"Id collision: {id}");
            }
            _objectsById[id] = gameObject;
        }

        public void Register(T gameObject, string name)
        {
            if (_objectsByName.ContainsKey(name))
            {
                throw new ArgumentException($"Name collision: {name}");
            }
            _objectsByName[name] = gameObject;
        }

        public void Register(T gameObject, int id, string name)
        {
            Register(gameObject, id);
            Register(gameObject, name);
        }
    }
}
