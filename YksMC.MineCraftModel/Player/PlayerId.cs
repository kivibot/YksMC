using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace YksMC.MinecraftModel.Player
{
    [DebuggerDisplay("{Value}")]
    public class PlayerId : IPlayerId
    {
        private readonly string _value;

        public string Value => _value;

        public PlayerId(string value)
        {
            _value = value;
        }

        public override bool Equals(object obj)
        {
            IPlayerId other = obj as IPlayerId;
            if(other == null)
            {
                return false;
            }
            return _value == other.Value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
