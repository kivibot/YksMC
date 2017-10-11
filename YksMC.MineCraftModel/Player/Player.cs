﻿using System;
using System.Collections.Generic;
using System.Text;

namespace YksMC.MinecraftModel.Player
{
    public class Player : IPlayer
    {
        private readonly Guid _id;
        private readonly int? _entityId;
        private readonly string _name;

        public Guid Id => _id;
        public int EntityId => _entityId.Value;
        public bool HasEntity => _entityId.HasValue;
        public string Name => _name;

        public Player(Guid id, string name)
            : this(id, name, null)
        {
        }

        public Player(Guid id, string name, int? entityId)
        {
            _id = id;
            _name = name;
            _entityId = entityId;
        }

        public IPlayer ChangeEntity(int entityId)
        {
            return new Player(_id, _name, entityId);
        }
    }
}